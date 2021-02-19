using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using SEB.SEBCS.RTM.v1.Client.Uakm463.Crossbordpmt.Update01.Fcpsts01;
using XB.Astrea.Client.Config;
using XB.Astrea.Client.Messages.Assessment;
using XB.Hubert;
using XB.Kafka;
using XB.MT.Parser.Parsers;
using Xunit;

namespace XB.Astrea.Client.Tests
{
    public class AstreaClientTests
    {

        //[Fact(Skip ="not ready")]
        //public void Parse_MtToAstreaRequest_ShouldReturnRequest()
        //{
        //    //var mt = MT103SingleCustomerCreditTransferParser.ParseMessage(AstreaClientTestConstants.Mt103);
        //    var parser = new MTParser();
        //    var mt = parser.ParseSwiftMt103Message(AstreaClientTestConstants.Mt103);
        //    var request = new AssessmentRequest(mt);

        //    var requestJson = TestHelper.SerializeToCamelCaseJson(request);

        //    Assert.True(request.Mt != string.Empty);
        //    Assert.True(requestJson != string.Empty);
        //}

        [Fact(Skip = "not ready")]
        public async Task Execute_AstreaAssessmentProcess_ShouldDoAssessment()
        {
            var httpClientFactoryMock = TestHelper.GetHttpClientFactoryMock();

            var hubertMock = new Mock<IHubertClient>();
            hubertMock.Setup(hubert =>
                hubert.SendAssessmentResponse(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>())).Returns(Task.FromResult(new CrossbordpmtUpdate01Fcpsts01Response()));
            var producerMock = new Mock<IKafkaProducer>();
            producerMock.Setup(producer =>
                producer.Produce(It.IsAny<string>())).Returns(Task.CompletedTask);

            var configurationMock = new Mock<IOptions<AstreaClientOptions>>();
            configurationMock.Setup(config => config.Value).Returns(new AstreaClientOptions { Version = AstreaClientTestConstants.Version });

            var astreaClient = new AstreaClient(httpClientFactoryMock.Object, producerMock.Object, configurationMock.Object, new Mock<ILogger<AstreaClient>>().Object, hubertMock.Object);

        //    var configurationMock = new Mock<IOptions<AtreaClientOptions>>();
        //    configurationMock.Setup(config => config.Value).Returns(new AtreaClientOptions { Version = AstreaClientTestConstants.Version });

            producerMock.Verify(mock =>
                mock.Produce(It.IsAny<string>()), Times.Once());
        }
    }

    internal static class TestHelper
    {
        internal static Mock<IHttpClientFactory> GetHttpClientFactoryMock()
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

        internal static string SerializeToCamelCaseJson(object value)
        {
            if (value == null) return "null";

            try
            {
                return JsonConvert.SerializeObject(value, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    Formatting = Formatting.Indented
                });
            }
            catch (Exception)
            {
                //log exception but dont throw one
                return "Exception";
            }
        }
    }
}
