using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using XB.Astrea.Client.Messages.Assessment;
using Moq;
using Moq.Protected;
using XB.Kafka;
using Xunit;
using XB.MT.Parser.Parsers;

namespace XB.Astrea.Client.Tests
{
    public class AstreaClientTests
    {

        [Fact]
        public void Parse_MtToAstreaRequest_ShouldReturnRequest()
        {
            var mt = MT103SingleCustomerCreditTransferParser.ParseMessage(AstreaClientTestConstants.Mt103);

            var request = Factory.GetAssessmentRequest(mt);

            var requestJson = request.ToJson();

            Assert.True(request.Mt != string.Empty);
            Assert.True(requestJson != string.Empty);
        }        
        
        [Fact]
        public async Task Execute_AstreaAssessmentProcess_ShouldDoAssessment()
        {
            var httpClientFactoryMock = HttpTestHelper.GetHttpClientFactoryMock();

            var producerMock = new Mock<IProducer>();
            producerMock.Setup(producer => 
                producer.Execute(It.IsAny<string>())).Returns(Task.CompletedTask);

            var astreaClient = new AstreaClient(httpClientFactoryMock.Object, producerMock.Object);

            await astreaClient.AssessAsync(AstreaClientTestConstants.Mt103);

            producerMock.Verify(mock => 
                mock.Execute(It.IsAny<string>()), Times.Once());
        }
    }

    public static class HttpTestHelper
    {
        public static Mock<IHttpClientFactory> GetHttpClientFactoryMock()
        {
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            httpClientFactoryMock.Setup(mock => 
                mock.CreateClient(It.IsAny<string>())).Returns(GetHttpClientMock());
            return httpClientFactoryMock;
        }

        private static HttpClient GetHttpClientMock()
        {
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
                .Protected()
                // Setup the PROTECTED method to mock
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                // prepare the expected response of the mocked http call
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(AstreaClientTestConstants.AstreaResponseAsString),
                })
                .Verifiable();

            // use real http client with mocked handler here
            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://test.com/"),
            };

            return httpClient;
        }
    }
}
