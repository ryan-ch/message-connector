﻿using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using XB.Astrea.Client.Config;
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

        private readonly AtreaClientOptions _config;
        private readonly HttpClient _httpClient;
        private readonly IKafkaProducer _kafkaProducer;
        private readonly ILogger<AstreaClient> _logger;

        public AstreaClient(IHttpClientFactory httpClientFactory, IKafkaProducer kafkaProducer, IOptions<AtreaClientOptions> config,
            ILogger<AstreaClient> logger)
        {
            _kafkaProducer = kafkaProducer;
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
                    if (!_config.AcceptableTransactionTypes.Any(format => mt.Contains("}{2:O" + format)))
                        return new AssessmentResponse();

                    return await HandleAssessAsync(mt).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error caught when trying to assess message, will retry: " + mt);
                    await Task.Delay(WaitingBeforeRetry).ConfigureAwait(false);
                }
            }
            _ = HandleTimeOutAsync(mt);
            _logger.LogError("Couldn't Handle this transaction message, stopped: " + mt);
            
            return new AssessmentResponse();
        }

        private async Task<AssessmentResponse> HandleTimeOutAsync(string mt)
        {
            var mt103 = GetPaymentInstruction(mt);
            var request = new AssessmentRequest(mt103);
            _logger.LogInformation("Sending to Hubert");
            var offeredTimeOut = new OfferedTimeOutProcessTrail(request, _config.Version);

            string offeredTimeOutProccessTrail = JsonConvert.SerializeObject(offeredTimeOut, ProcessTrailDefaultJsonSettings.Settings);
            _logger.LogInformation("Printing OfferedTimeOutProcessTrail: " + offeredTimeOutProccessTrail);
            
            return new AssessmentResponse();
            
        }

        private async Task<AssessmentResponse> HandleAssessAsync(string mt)
        {
            var mt103 = GetPaymentInstruction(mt);
            var request = new AssessmentRequest(mt103);
            var postBody = JsonConvert.SerializeObject(request, ProcessTrailDefaultJsonSettings.Settings);
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
                var requestedProcessTrail = new RequestedProcessTrail(request, _config.Version);
                string kafkaMessage = JsonConvert.SerializeObject(requestedProcessTrail, ProcessTrailDefaultJsonSettings.Settings);
                _logger.LogInformation("Sending RequestedProcessTrail: " + kafkaMessage);
                await _kafkaProducer.Produce(kafkaMessage).ConfigureAwait(false);
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
                var kafkaMessage = (riskLevel > _config.RiskThreshold)
                       ? JsonConvert.SerializeObject(new RejectedProcessTrail(assessmentResponse, _config.Version, parsedMt), ProcessTrailDefaultJsonSettings.Settings)
                       : JsonConvert.SerializeObject(new OfferedProcessTrail(assessmentResponse, _config.Version, parsedMt), ProcessTrailDefaultJsonSettings.Settings);

                _logger.LogInformation("Sending DecisionProcessTrail: " + kafkaMessage);
                await _kafkaProducer.Produce(kafkaMessage).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Couldn't Send Decision ProcessTrail for response: " + JsonConvert.SerializeObject(assessmentResponse));
            }
        }
    }
}
