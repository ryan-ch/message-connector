using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using SEB.SEBCS.RTM.v1.Client.Uakm463;
using SEB.SEBCS.RTM.v1.Client.Uakm463.Crossbordpmt.Update01.Fcpsts01;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Testing.Common;
using Testing.Common.Test_Data;
using XB.Astrea.Client.Config;
using XB.Astrea.Client.Messages.Assessment;
using XB.Hubert;
using XB.Kafka;
using XB.MtParser.Interfaces;
using XB.MtParser.Mt103;
using Xunit;

namespace XB.Astrea.Client.Tests
{
    public class AstreaClientUnitTests
    {
        private readonly AssessmentResponse _expectedResultObject = new AssessmentResponse { Identity = "abc0123", RiskLevel = "4", Results = new List<AssessmentResult>() };

        private readonly CrossbordpmtUpdate01Fcpsts01Response _hubertResponse = new CrossbordpmtUpdate01Fcpsts01Response
        { Result = new CrossbordpmtUpdate01Fcpsts01Tables { Uakw4630 = new UAKW4630 { TransactionStatus = "Accepted" } } };

        private readonly Mock<IMTParser> _mTParserMock;
        private readonly Mock<IKafkaProducer> _kafkaProducerMock;
        private readonly Mock<ILogger<AstreaClient>> _loggerMock;
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
        private readonly Mock<HttpMessageHandler> _messageHandlerMock;
        private readonly Mock<IOptions<AstreaClientOptions>> _configMock;
        private readonly Mock<IHubertClient> _huberClientMock;

        private readonly AstreaClient _astreaClient;

        public AstreaClientUnitTests()
        {
            _kafkaProducerMock = new Mock<IKafkaProducer>();
            _loggerMock = new Mock<ILogger<AstreaClient>>();

            _mTParserMock = new Mock<IMTParser>();
            _mTParserMock.Setup(a => a.ParseSwiftMt103Message(SwiftMessagesMock.SwiftMessage_2.OriginalMessage))
                .Returns(new Mt103Message(SwiftMessagesMock.SwiftMessage_2.OriginalMessage, null));

            _configMock = new Mock<IOptions<AstreaClientOptions>>();
            _configMock.Setup(c => c.Value)
                .Returns(new AstreaClientOptions { RetryPeriodInMin = 0.03, WaitingBeforeRetryInSec = 1, AcceptableTransactionTypes = new List<string> { "103" }, RiskThreshold = 3, Version = "1.0" });

            (_httpClientFactoryMock, _messageHandlerMock) = TestUtilities.GetHttpClientFactoryMock(JsonConvert.SerializeObject(_expectedResultObject));

            _huberClientMock = new Mock<IHubertClient>();
            _huberClientMock.Setup(a => a.SendAssessmentResultAsync(It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(_hubertResponse);

            _astreaClient = new AstreaClient(_httpClientFactoryMock.Object, _kafkaProducerMock.Object,
                _configMock.Object, _loggerMock.Object, _mTParserMock.Object, _huberClientMock.Object);
        }

        [Fact]
        public void AstreaClient_ShouldCreateHttpClientFromFactory_WithCorrectName()
        {
            // Arrange
            // Act
            // Assert
            _httpClientFactoryMock.Verify(a => a.CreateClient(AstreaClientExtensions.HttpClientName), Times.Once);
        }

        [Fact]
        public async Task AssessAsync_WillReturnEmpty_IfTransactionTypeIsNotAccepted()
        {
            // Arrange
            var message = SwiftMessagesMock.SwiftMessage_2.OriginalMessage.Replace("{2:O103", "{2:O102");

            // Act
            var result = await _astreaClient.AssessAsync(message).ConfigureAwait(false);

            // Assert
            Assert.Equal(new AssessmentResponse(), result);
        }

        [Fact]
        public async Task AssessAsync_WillParseTheMessage_SendAssessRequestAndReturnResult()
        {
            // Arrange
            // Act
            var result = await _astreaClient.AssessAsync(SwiftMessagesMock.SwiftMessage_2.OriginalMessage).ConfigureAwait(false);

            // Assert
            _mTParserMock.Verify(a => a.ParseSwiftMt103Message(SwiftMessagesMock.SwiftMessage_2.OriginalMessage), Times.Once);
            _messageHandlerMock.Protected().Verify("SendAsync", Times.Once(), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
            Assert.Equal(_expectedResultObject.ToString(), result.ToString());
        }

        [Fact]
        public async Task AssessAsync_WhenErrorThrownFromParser_WillLogItAndReturn()
        {
            // Arrange
            var ex = new Exception("Test Exception");
            _mTParserMock.Setup(a => a.ParseSwiftMt103Message(SwiftMessagesMock.SwiftMessage_2.OriginalMessage))
                .Throws(ex);

            // Act
            var result = await _astreaClient.AssessAsync(SwiftMessagesMock.SwiftMessage_2.OriginalMessage).ConfigureAwait(false);

            // Assert
            _mTParserMock.Verify(a => a.ParseSwiftMt103Message(SwiftMessagesMock.SwiftMessage_2.OriginalMessage), Times.Once);
            _messageHandlerMock.Protected().Verify("SendAsync", Times.Never(), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
            _loggerMock.VerifyLoggerCall(LogLevel.Error, "Couldn't parse the mt103 message: ", Times.Once(), ex);
            _kafkaProducerMock.Verify(a => a.Produce(It.IsAny<string>()), Times.Never);
            Assert.Equal(new AssessmentResponse(), result);
        }

        [Fact]
        public async Task AssessAsync_WhenAstreaFail_WillLogItAndRetry()
        {
            // Arrange
            var (httpClientFactoryMock, messageHandlerMock) = TestUtilities.GetHttpClientFactoryMock(JsonConvert.SerializeObject(_expectedResultObject),
                HttpStatusCode.InternalServerError);
            var astreaClient = new AstreaClient(httpClientFactoryMock.Object, _kafkaProducerMock.Object,
                _configMock.Object, _loggerMock.Object, _mTParserMock.Object, _huberClientMock.Object);

            // Act
            var result = await astreaClient.AssessAsync(SwiftMessagesMock.SwiftMessage_2.OriginalMessage).ConfigureAwait(false);

            // Assert
            _mTParserMock.Verify(a => a.ParseSwiftMt103Message(SwiftMessagesMock.SwiftMessage_2.OriginalMessage), Times.Once);
            messageHandlerMock.Protected().Verify("SendAsync", Times.Exactly(2), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
            _loggerMock.VerifyLoggerCall(LogLevel.Error, "Error caught when trying to assess message", Times.Exactly(2));
            _loggerMock.VerifyLoggerCall(LogLevel.Error, "Couldn't Handle this transaction message", Times.Once());
            _kafkaProducerMock.Verify(a => a.Produce(It.Is<string>(s => s.Contains(JsonConvert.SerializeObject(SwiftMessagesMock.SwiftMessage_2.OriginalMessage)))),
                Times.Exactly(3));
            Assert.Equal(new AssessmentResponse(), result);
        }

        [Fact]
        public async Task AssessAsync_WillSendRequestedProcessTrailAndLogIt()
        {
            // Arrange
            // Act
            _ = await _astreaClient.AssessAsync(SwiftMessagesMock.SwiftMessage_2.OriginalMessage).ConfigureAwait(false);

            // Assert
            _kafkaProducerMock.Verify(a => a.Produce(It.Is<string>(s => s.Contains(JsonConvert.SerializeObject(SwiftMessagesMock.SwiftMessage_2.OriginalMessage)))), Times.Once);
            _loggerMock.VerifyLoggerCall(LogLevel.Information, "Sending RequestedProcessTrail", Times.Once());
        }

        [Fact]
        public async Task AssessAsync_WillSendDecisionProcessTrailAndLogIt()
        {
            // Arrange
            // Act
            _ = await _astreaClient.AssessAsync(SwiftMessagesMock.SwiftMessage_2.OriginalMessage).ConfigureAwait(false);

            // Assert
            _kafkaProducerMock.Verify(a => a.Produce(It.Is<string>(s => s.Contains(_expectedResultObject.Identity))), Times.Once);
            _loggerMock.VerifyLoggerCall(LogLevel.Information, "Sending DecisionProcessTrail", Times.Once());
        }

        [Fact]
        public async Task AssessAsync_WillLogError_WhenSendingProcessTrailsFail()
        {
            // Arrange
            var ex = new Exception("Test Exception");
            _kafkaProducerMock.Setup(a => a.Produce(It.IsAny<string>()))
                .Throws(ex);

            // Act
            _ = await _astreaClient.AssessAsync(SwiftMessagesMock.SwiftMessage_2.OriginalMessage).ConfigureAwait(false);

            // Assert
            _kafkaProducerMock.Verify(a => a.Produce(It.IsAny<string>()), Times.Exactly(2));
            _loggerMock.VerifyLoggerCall(LogLevel.Information, "Sending RequestedProcessTrail", Times.Once());
            _loggerMock.VerifyLoggerCall(LogLevel.Information, "Sending DecisionProcessTrail", Times.Once());

            _loggerMock.VerifyLoggerCall(LogLevel.Error, "Couldn't Send Requested ProcessTrail for request", Times.Once(), ex);
            _loggerMock.VerifyLoggerCall(LogLevel.Error, "Couldn't Send Decision ProcessTrail for response", Times.Once(), ex);
        }
    }
}
