using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using XB.Astrea.Client.Config;
using XB.Astrea.Client.Constants;
using XB.Astrea.Client.Messages.Assessment;
using XB.Astrea.Client.Messages.ProcessTrail;
using XB.Hubert;
using XB.Kafka;
using XB.MtParser.Interfaces;
using XB.MtParser.Mt103;

namespace XB.Astrea.Client
{
    public class AstreaClient : IAstreaClient
    {
        private readonly AstreaClientOptions _config;
        private readonly HttpClient _httpClient;
        private readonly IKafkaProducer _kafkaProducer;
        private readonly IMTParser _mTParser;
        private readonly ILogger<AstreaClient> _logger;
        private readonly IHubertClient _hubertClient;

        public AstreaClient(IHttpClientFactory httpClientFactory, IKafkaProducer kafkaProducer, IOptions<AstreaClientOptions> config,
            ILogger<AstreaClient> logger, IMTParser mTParser, IHubertClient hubertClient)
        {
            _kafkaProducer = kafkaProducer;
            _mTParser = mTParser;
            _logger = logger;
            _config = config.Value;
            _httpClient = httpClientFactory.CreateClient(AstreaClientExtensions.HttpClientName);
            _hubertClient = hubertClient;
        }

        public async Task<AssessmentResponse> AssessAsync(string mt)
        {
            var finishTimestamp = DateTime.Now.AddMinutes(_config.RetryPeriodInMin);
            var receivedAt = DateTime.Now;

            while (DateTime.Now <= finishTimestamp)
            {
                try
                {
                    // Todo: Check the performance of the next line (maybe replace with parsed object check)
                    // Todo: Can it be I instead of O?
                    if (!_config.AcceptableTransactionTypes.Any(format => mt.Contains("}{2:O" + format)))
                        return new AssessmentResponse();

                    return await HandleAssessAsync(mt, receivedAt).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error caught when trying to assess message, will retry: {mt}", mt);
                    await Task.Delay(Convert.ToInt32(_config.WaitingBeforeRetryInSec * 1000)).ConfigureAwait(false);
                }
            }
            _ = HandleTimeOutAsync(mt, receivedAt);
            _logger.LogError("Couldn't Handle this transaction message, stopped: " + mt);

            return new AssessmentResponse();
        }
        //TODO: Refactor logic in this method. Should we send to Hubert? Do we need to wait for the response from Hubert? etc
        private async Task HandleTimeOutAsync(string mt, DateTime receivedAt)
        {
            //TODO: Refactor the two lines below
            var mt103 = _mTParser.ParseSwiftMt103Message(mt);
            var request = new AssessmentRequest(mt103);
            try
            {
                var hubertStatus =await SendToHubert(AstreaClientConstants.Hubert_Timeout, mt103, receivedAt).ConfigureAwait(false);
                if (hubertStatus == AstreaClientConstants.Hubert_Accepted)
                {
                    var offeredTimeOut = new OfferedTimeOutProcessTrail(request, _config.Version);

                    var offeredTimeOutProcessTrail = JsonConvert.SerializeObject(offeredTimeOut, ProcessTrailDefaultJsonSettings.Settings);
                    _logger.LogInformation("Sending OfferedTimeOutProcessTrail: " + offeredTimeOutProcessTrail);

                    await _kafkaProducer.Produce(offeredTimeOutProcessTrail).ConfigureAwait(false);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Couldn't Send OfferedTimeOutProcessTrail for request: " + JsonConvert.SerializeObject(request, ProcessTrailDefaultJsonSettings.Settings));
            }
        }

        private async Task<AssessmentResponse> HandleAssessAsync(string mt, DateTime receivedAt)
        {
            var mt103 = _mTParser.ParseSwiftMt103Message(mt);
            var request = new AssessmentRequest(mt103);
            var postBody = JsonConvert.SerializeObject(request, ProcessTrailDefaultJsonSettings.Settings);
            var data = new StringContent(postBody, Encoding.UTF8, "application/json");

            _ = SendRequestedProcessTrail(request);
            var result = await _httpClient.PostAsync("/sas/v3/assessOrders/paymentInstruction", data).ConfigureAwait(false);

            var apiResponse = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (!result.IsSuccessStatusCode)
                throw new Exception("Request to Astrea API could not be completed, returned code: " + result.StatusCode + ". Response from API: " + apiResponse);

            var assessmentResponse = JsonConvert.DeserializeObject<AssessmentResponse>(apiResponse);

            var hubertStatus = await SendToHubert(assessmentResponse.RiskLevel, mt103, receivedAt).ConfigureAwait(false);
            _ = SendDecisionProcessTrail(hubertStatus, assessmentResponse, mt103);

            return assessmentResponse;
        }

        private async Task<string> SendToHubert(string status, Mt103Message parsedMt, DateTime receivedAt)
        {
            var hubertResponse = await _hubertClient.SendAssessmentResultAsync(receivedAt.ToString(), parsedMt.UserHeader.UniqueEndToEndTransactionReference, status);
            return hubertResponse.Result.Uakw4630.TransactionStatus.ToUpper();
        }

        private async Task SendRequestedProcessTrail(AssessmentRequest request)
        {
            try
            {
                var requestedProcessTrail = new RequestedProcessTrail(request, _config.Version);
                var kafkaMessage = JsonConvert.SerializeObject(requestedProcessTrail, ProcessTrailDefaultJsonSettings.Settings);
                _logger.LogInformation("Sending RequestedProcessTrail: {kafkaMessage}", kafkaMessage);
                await _kafkaProducer.Produce(kafkaMessage).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Couldn't Send Requested ProcessTrail for request: {request}", request);
            }
        }

        private async Task SendDecisionProcessTrail(string hubertStatus, AssessmentResponse assessmentResponse, Mt103Message parsedMt)
        {
            try
            {
                var kafkaMessage = hubertStatus == AstreaClientConstants.Hubert_Rejected
                     ? JsonConvert.SerializeObject(new RejectedProcessTrail(assessmentResponse, _config.Version, parsedMt), ProcessTrailDefaultJsonSettings.Settings)
                     : JsonConvert.SerializeObject(new OfferedProcessTrail(assessmentResponse, _config.Version, parsedMt, hubertStatus == AstreaClientConstants.Hubert_Timeout), ProcessTrailDefaultJsonSettings.Settings);

                _logger.LogInformation("Sending DecisionProcessTrail: {kafkaMessage}", kafkaMessage);
                await _kafkaProducer.Produce(kafkaMessage).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Couldn't Send Decision ProcessTrail for response: {assessmentResponse}", assessmentResponse);
            }
        }
    }
}
