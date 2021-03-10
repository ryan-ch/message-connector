using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
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

        public async Task<AssessmentResponse> AssessAsync(string originalMessage)
        {
            if (!ValidMessageType(originalMessage))
                return new AssessmentResponse();

            var finishTimestamp = DateTime.Now.AddMinutes(_config.RetryPeriodInMin);
            var receivedAt = DateTime.Now;
            AssessmentRequest assessmentRequest;
            Mt103Message mt103;

            try
            {
                mt103 = _mTParser.ParseSwiftMt103Message(originalMessage);
                assessmentRequest = new AssessmentRequest(mt103);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Couldn't parse the mt103 message: {originalMessage}", originalMessage);
                return new AssessmentResponse();
            }

            while (DateTime.Now <= finishTimestamp)
            {
                try
                {
                    return await HandleAssessAsync(assessmentRequest, mt103, receivedAt).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error caught when trying to assess message, will retry: {mt103}", mt103);
                    await Task.Delay(Convert.ToInt32(_config.WaitingBeforeRetryInSec * 1000)).ConfigureAwait(false);
                }
            }
            _ = HandleTimeOutAsync(assessmentRequest, mt103.UserHeader.UniqueEndToEndTransactionReference, receivedAt);
            _logger.LogError("Couldn't Handle this transaction message, stopped: {originalMessage}", originalMessage);

            return new AssessmentResponse();
        }

        private bool ValidMessageType(string originalMessage)
        {
            foreach (var t in _config.AcceptableTransactionTypes)
                if (originalMessage.Contains("}{2:O" + t))
                    return true;

            return false;
        }

        private async Task<AssessmentResponse> HandleAssessAsync(AssessmentRequest request, Mt103Message mt103, DateTime receivedAt)
        {
            var postBody = JsonConvert.SerializeObject(request, AstreaClientOptions.ProcessTrailDefaultJsonSettings);
            var data = new StringContent(postBody, Encoding.UTF8, "application/json");

            _ = SendRequestedProcessTrail(request);
            var result = await _httpClient.PostAsync("/sas/v3/assessOrders/paymentInstruction", data).ConfigureAwait(false);

            var apiResponse = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (!result.IsSuccessStatusCode)
                throw new Exception("Request to Astrea API could not be completed, returned code: " + result.StatusCode + ". Response from API: " + apiResponse);

            var assessmentResponse = JsonConvert.DeserializeObject<AssessmentResponse>(apiResponse);

            var hubertStatus = await SendToHubert(assessmentResponse.RiskLevel, mt103.UserHeader.UniqueEndToEndTransactionReference, receivedAt).ConfigureAwait(false);
            _ = SendDecisionProcessTrail(hubertStatus, assessmentResponse, mt103);

            return assessmentResponse;
        }

        //TODO: Refactor logic in this method. Should we send to Hubert? Do we need to wait for the response from Hubert? etc
        private async Task HandleTimeOutAsync(AssessmentRequest request, string transactionReference, DateTime receivedAt)
        {
            try
            {
                _ = SendToHubert(AstreaClientConstants.Hubert_Timeout, transactionReference, receivedAt).ConfigureAwait(false);

                var offeredTimeOut = new OfferedTimeOutProcessTrail(request, _config.Version);

                var offeredTimeOutProcessTrail = JsonConvert.SerializeObject(offeredTimeOut, AstreaClientOptions.ProcessTrailDefaultJsonSettings);
                _logger.LogInformation("Sending OfferedTimeOutProcessTrail: {offeredTimeOutProcessTrail}", offeredTimeOutProcessTrail);

                await _kafkaProducer.Produce(offeredTimeOutProcessTrail).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Couldn't Send OfferedTimeOutProcessTrail for request: {request}", request);
            }
        }

        private async Task<string> SendToHubert(string status, string transactionReference, DateTime receivedAt)
        {
            var hubertResponse = await _hubertClient.SendAssessmentResultAsync(receivedAt, transactionReference, status);
            return hubertResponse.Result.Uakw4630.TransactionStatus.ToUpper();
        }

        private async Task SendRequestedProcessTrail(AssessmentRequest request)
        {
            try
            {
                var requestedProcessTrail = new RequestedProcessTrail(request, _config.Version);
                var kafkaMessage = JsonConvert.SerializeObject(requestedProcessTrail, AstreaClientOptions.ProcessTrailDefaultJsonSettings);
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
                     ? JsonConvert.SerializeObject(new RejectedProcessTrail(assessmentResponse, _config.Version, parsedMt), AstreaClientOptions.ProcessTrailDefaultJsonSettings)
                     : JsonConvert.SerializeObject(new OfferedProcessTrail(assessmentResponse, _config.Version, parsedMt, hubertStatus == AstreaClientConstants.Hubert_Timeout),
                         AstreaClientOptions.ProcessTrailDefaultJsonSettings);

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
