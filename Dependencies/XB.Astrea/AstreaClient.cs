using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using XB.Astrea.Client.Exceptions;
using XB.Astrea.Client.Messages.Assessment;
using XB.Astrea.Client.Messages.ProcessTrail;
using XB.Kafka;
using XB.MT.Parser.Model;
using XB.MT.Parser.Parsers;

namespace XB.Astrea.Client
{
    public class AstreaClient : IAstreaClient
    {
        private const int WaitingBeforeRetry = 60000;
        private readonly DateTime _timeStamp = DateTime.Now;

        private readonly IKafkaProducer _kafkaProducer;
        private readonly HttpClient _httpClient;
        private readonly ILogger<AstreaClient> _logger;
        private readonly string _appVersion;
        private readonly byte _retryPeriod;
        private readonly int _riskThreshold;

        public AstreaClient(IHttpClientFactory httpClientFactory, IKafkaProducer kafkaProducer, IConfiguration configuration,
            ILogger<AstreaClient> logger)
        {
            _kafkaProducer = kafkaProducer;
            _logger = logger;
            _appVersion = configuration.GetValue("Version", string.Empty);
            _retryPeriod = configuration.GetValue("AppSettings:Astrea:RetryPeriod", (byte)5);
            _httpClient = httpClientFactory.CreateClient(AstreaClientExtensions.HttpClientName);
            //TODO: Store this in config after issues with processing different messages have been resolved
            _riskThreshold = configuration.GetValue("RiskThreshold", 5);
        }

        public async Task<AssessmentResponse> AssessAsync(string mt)
        {
            try
            {
                return await HandleAssessAsync(mt).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                while (DateTime.Now <= _timeStamp.AddMinutes(_retryPeriod))
                {
                    try
                    {
                        return await HandleAssessAsync(mt).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error caught when handling message: " + mt);
                        await Task.Delay(WaitingBeforeRetry).ConfigureAwait(false);
                    }
                }
                _logger.LogError(e, "Couldn't Handle this transaction message: " + mt);
                return new AssessmentResponse();
            }
        }

        private async Task<AssessmentResponse> HandleAssessAsync(string mt)
        {
            var mt103 = MT103SingleCustomerCreditTransferParser.ParseMessage(mt);
            var request = new AssessmentRequest(mt103);
            var postBody = JsonConvert.SerializeObject(request, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            });
            var data = new StringContent(postBody, Encoding.UTF8, "application/json");
            var result = await _httpClient.PostAsync("/sas/v3/assessOrders/paymentInstruction", data).ConfigureAwait(false);

            if (!result.IsSuccessStatusCode)
                throw new AssessmentErrorException("Request to Astrea API could not be completed, returned code: " + result.StatusCode);

            // Todo: do we need to await for this one ?
            _ = SendRequestedProcessTrail(request);

            var apiResponse = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
            var assessmentResponse = JsonConvert.DeserializeObject<AssessmentResponse>(apiResponse);

            // TODO: Here we need to make a decision if this is an offered or rejected assessment
            _ = SendDecisionProcessTrail(assessmentResponse, mt103);

            //Todo: we need to send the result to Hubert
            return assessmentResponse;
        }

        private Task SendRequestedProcessTrail(AssessmentRequest request)
        {
            try
            {
                var requestedProcessTrail = new RequestedProcessTrail(request, _appVersion);
                _ = _kafkaProducer.Execute(JsonConvert.SerializeObject(requestedProcessTrail));
            }
            catch (Exception e)
            {
                // Todo: we shouldn't throw if the logging failed we should log it only
                //throw new ProcessTrailErrorException("Could not send requested process trail", e);
                _logger.LogError(e, "Couldn't Send Requested ProcessTrail");
            }
            return Task.CompletedTask;
        }

        private Task SendDecisionProcessTrail(AssessmentResponse assessmentResponse, MT103SingleCustomerCreditTransferModel parsedMt)
        {
            try
            {
                var riskLevel = int.Parse(assessmentResponse.RiskLevel);
                var kafkaMessage = (riskLevel > _riskThreshold)
                       ? JsonConvert.SerializeObject(new RejectedProcessTrail(assessmentResponse, _appVersion, parsedMt))
                       : JsonConvert.SerializeObject(new OfferedProcessTrail(assessmentResponse, _appVersion, parsedMt));

                _ = _kafkaProducer.Execute(kafkaMessage);
            }
            catch (Exception e)
            {
                // Todo: we shouldn't throw if the logging failed we should log it only
                //throw new ProcessTrailErrorException("Could not send decision process trail", e);
                _logger.LogError(e, "Couldn't Send Decision ProcessTrail");
            }
            return Task.CompletedTask;
        }
    }
}
