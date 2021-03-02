using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using XB.Astrea.Client.Config;
using XB.Astrea.Client.Messages.Assessment;
using XB.Astrea.Client.Messages.ProcessTrail;
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

        public AstreaClient(IHttpClientFactory httpClientFactory, IKafkaProducer kafkaProducer, IOptions<AstreaClientOptions> config, IMTParser mTParser,
            ILogger<AstreaClient> logger)
        {
            _kafkaProducer = kafkaProducer;
            _mTParser = mTParser;
            _logger = logger;
            _config = config.Value;
            _httpClient = httpClientFactory.CreateClient(AstreaClientExtensions.HttpClientName);
        }

        public async Task<AssessmentResponse> AssessAsync(string mt)
        {
            var finishTimestamp = DateTime.Now.AddMinutes(_config.RetryPeriodInMin);

            while (DateTime.Now <= finishTimestamp)
            {
                try
                {
                    // Todo: Check the performance of the next line (maybe replace with parsed object check)
                    // Todo: Can it be I instead of O?
                    if (!_config.AcceptableTransactionTypes.Any(format => mt.Contains("}{2:O" + format)))
                        return new AssessmentResponse();

                    return await HandleAssessAsync(mt).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error caught when trying to assess message: {mt}", mt);
                    await Task.Delay(Convert.ToInt32(_config.WaitingBeforeRetryInSec * 1000)).ConfigureAwait(false);
                }
            }
            _logger.LogError("Couldn't Handle this transaction message: {mt}", mt);
            return new AssessmentResponse();
        }

        private async Task<AssessmentResponse> HandleAssessAsync(string mt)
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

            _ = SendDecisionProcessTrail(assessmentResponse, mt103);

            //Todo: we need to send the result to Hubert
            return assessmentResponse;
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

        private async Task SendDecisionProcessTrail(AssessmentResponse assessmentResponse, Mt103Message parsedMt)
        {
            try
            {
                var riskLevel = int.Parse(assessmentResponse.RiskLevel);
                var kafkaMessage = riskLevel > _config.RiskThreshold
                       ? JsonConvert.SerializeObject(new RejectedProcessTrail(assessmentResponse, _config.Version, parsedMt), ProcessTrailDefaultJsonSettings.Settings)
                       : JsonConvert.SerializeObject(new OfferedProcessTrail(assessmentResponse, _config.Version, parsedMt), ProcessTrailDefaultJsonSettings.Settings);

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
