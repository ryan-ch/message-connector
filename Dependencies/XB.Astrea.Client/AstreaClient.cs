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
            var messageType = GetMessageType(originalMessage);
            if (!_config.AcceptableTransactionTypes.Contains(messageType))
            {
                _logger.LogWarning("Message is not supported, ignoring message with type: {messageType}", messageType);
                return new AssessmentResponse();
            }

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

            try
            {
                return await HandleAssessAsync(assessmentRequest, mt103, receivedAt);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Couldn't assess this transaction message: {originalMessage}", originalMessage);
                _ = HandleAstreaFailAsync(assessmentRequest, mt103.UserHeader.UniqueEndToEndTransactionReference, receivedAt);
            }

            return new AssessmentResponse();
        }

        private string GetMessageType(string originalMessage)
        {
            var secondBlockIndex = originalMessage.IndexOf("}{2:O");
            return secondBlockIndex == -1
                ? originalMessage
                : originalMessage.Substring(secondBlockIndex + 5, 3);
        }

        private async Task<AssessmentResponse> HandleAssessAsync(AssessmentRequest request, Mt103Message mt103, DateTime receivedAt)
        {
            var postBody = JsonConvert.SerializeObject(request, AstreaClientOptions.ProcessTrailDefaultJsonSettings);
            var data = new StringContent(postBody, Encoding.UTF8, "application/json");

            _ = SendRequestedProcessTrail(request);
            var result = await _httpClient.PostAsync(_config.FraudEndpoint, data);

            var apiResponse = await result.Content.ReadAsStringAsync();

            if (!result.IsSuccessStatusCode)
                throw new Exception("Request to Astrea API could not be completed, returned code: " + result.StatusCode + ". Response from API: " + apiResponse);

            var assessmentResponse = JsonConvert.DeserializeObject<AssessmentResponse>(apiResponse);

            var hubertStatus = await SendToHubert(assessmentResponse.RiskLevel, mt103.UserHeader.UniqueEndToEndTransactionReference, receivedAt);
            _ = SendDecisionProcessTrail(hubertStatus, assessmentResponse, mt103);

            return assessmentResponse;
        }

        private async Task HandleAstreaFailAsync(AssessmentRequest request, string transactionReference, DateTime receivedAt)
        {
            try
            {
                _ = SendToHubert(AstreaClientConstants.Hubert_Timeout, transactionReference, receivedAt);

                var offeredTimeOut = new OfferedTimeOutProcessTrail(request, _config.Version);

                var offeredTimeOutProcessTrail = JsonConvert.SerializeObject(offeredTimeOut, AstreaClientOptions.ProcessTrailDefaultJsonSettings);
                _logger.LogInformation("Sending OfferedTimeOutProcessTrail: {offeredTimeOutProcessTrail}", offeredTimeOutProcessTrail);

                await _kafkaProducer.Produce(offeredTimeOutProcessTrail);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Couldn't Send OfferedTimeOutProcessTrail for request: {request}", request);
            }
        }

        private async Task<string> SendToHubert(string status, string transactionReference, DateTime receivedAt)
        {
            try
            {
                var hubertResponse = await _hubertClient.SendAssessmentResultAsync(receivedAt, transactionReference, status);
                return hubertResponse.Result.Uakw4630.TransactionStatus.ToUpper();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Couldn't contact Hubert for transactionReference: {transactionReference}", transactionReference);
                return AstreaClientConstants.Hubert_Timeout;
            }
        }

        private async Task SendRequestedProcessTrail(AssessmentRequest request)
        {
            try
            {
                var requestedProcessTrail = new RequestedProcessTrail(request, _config.Version);
                var kafkaMessage = JsonConvert.SerializeObject(requestedProcessTrail, AstreaClientOptions.ProcessTrailDefaultJsonSettings);
                _logger.LogInformation("Sending RequestedProcessTrail: {kafkaMessage}", kafkaMessage);
                await _kafkaProducer.Produce(kafkaMessage);
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
                     ? JsonConvert.SerializeObject(new RejectedProcessTrail(assessmentResponse, _config.Version, parsedMt, hubertStatus), AstreaClientOptions.ProcessTrailDefaultJsonSettings)
                     : JsonConvert.SerializeObject(new OfferedProcessTrail(assessmentResponse, _config.Version, parsedMt, hubertStatus), AstreaClientOptions.ProcessTrailDefaultJsonSettings);

                _logger.LogInformation("Sending DecisionProcessTrail: {kafkaMessage}", kafkaMessage);
                await _kafkaProducer.Produce(kafkaMessage);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Couldn't Send Decision ProcessTrail for response: {assessmentResponse}", assessmentResponse);
            }
        }
    }
}
