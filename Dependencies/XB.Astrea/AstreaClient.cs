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

        private readonly string _appVersion;
        private readonly byte _retryPeriod;
        private readonly int _riskThreshold;
        private readonly HttpClient _httpClient;
        private readonly IKafkaProducer _kafkaProducer;
        private readonly ILogger<AstreaClient> _logger;

        public AstreaClient(IHttpClientFactory httpClientFactory, IKafkaProducer kafkaProducer, IConfiguration configuration,
            ILogger<AstreaClient> logger)
        {
            _kafkaProducer = kafkaProducer;
            _logger = logger;
            _appVersion = configuration.GetValue("Version", string.Empty);
            _retryPeriod = configuration.GetValue("AppSettings:Astrea:RetryPeriod", (byte)15);
            _httpClient = httpClientFactory.CreateClient(AstreaClientExtensions.HttpClientName);
            _riskThreshold = configuration.GetValue("RiskThreshold", 5);
        }

        public async Task<AssessmentResponse> AssessAsync(string mt)
        {
            var finishTimestamp = DateTime.Now.AddMinutes(_retryPeriod);

            while (DateTime.Now <= finishTimestamp)
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
            _logger.LogError("Couldn't Handle this transaction message: " + mt);
            return new AssessmentResponse();
        }

        private async Task<AssessmentResponse> HandleAssessAsync(string mt)
        {
            var mt103 = GetPaymentInstruction(mt);
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

            _ = SendRequestedProcessTrail(request);

            var apiResponse = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
            var assessmentResponse = JsonConvert.DeserializeObject<AssessmentResponse>(apiResponse);

            _ = SendDecisionProcessTrail(assessmentResponse, mt103);

            //Todo: we need to send the result to Hubert
            return assessmentResponse;
        }

        private MT103SingleCustomerCreditTransferModel GetPaymentInstruction(string mt)
        {
            var parser = new MT103SingleCustomerCreditTransferParser();
            return parser.ParseMessage(mt);
        }

        private async Task SendRequestedProcessTrail(AssessmentRequest request)
        {
            try
            {
                var requestedProcessTrail = new RequestedProcessTrail(request, _appVersion);
                string kafkaMessage = JsonConvert.SerializeObject(requestedProcessTrail);

                _logger.LogInformation("Sending RequestedProcessTrail: " + kafkaMessage);
                await _kafkaProducer.Execute(kafkaMessage).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Couldn't Send Requested ProcessTrail for request: " + JsonConvert.SerializeObject(request));
            }
        }

        private async Task SendDecisionProcessTrail(AssessmentResponse assessmentResponse, MT103SingleCustomerCreditTransferModel parsedMt)
        {
            try
            {
                var riskLevel = int.Parse(assessmentResponse.RiskLevel);
                var kafkaMessage = (riskLevel > _riskThreshold)
                       ? JsonConvert.SerializeObject(new RejectedProcessTrail(assessmentResponse, _appVersion, parsedMt))
                       : JsonConvert.SerializeObject(new OfferedProcessTrail(assessmentResponse, _appVersion, parsedMt));

                _logger.LogInformation("Sending DecisionProcessTrail: " + kafkaMessage);
                await _kafkaProducer.Execute(kafkaMessage).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Couldn't Send Decision ProcessTrail for response: " + JsonConvert.SerializeObject(assessmentResponse));
            }
        }
    }
}
