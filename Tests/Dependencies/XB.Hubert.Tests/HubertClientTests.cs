using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using SEB.SEBCS.RTM.v1.Client.Uakm463;
using SEB.SEBCS.RTM.v1.Client.Uakm463.Crossbordpmt.Update01.Fcpsts01;
using XB.Hubert.Config;
using Xunit;

namespace XB.Hubert.Tests
{
    public class HubertClientTests
    {
        private readonly Mock<IOptions<HubertClientOptions>> _configMock;
        private readonly Mock<HttpClient> _httpClientMock;
        private readonly Mock<ILogger<HubertClient>> _loggerMock;
        private readonly Mock<IHttpClientFactory> _clientFactoryMock;
        private readonly IHubertClient _hubertClient;
        private readonly HubertClientOptions _hubertClientOptions;

        public HubertClientTests()
        {
            _configMock = new Mock<IOptions<HubertClientOptions>>();
            _httpClientMock = new Mock<HttpClient>();
            _loggerMock = new Mock<ILogger<HubertClient>>();
            _clientFactoryMock = new Mock<IHttpClientFactory>();
            _hubertClientOptions = new HubertClientOptions()
            {
                Url = "https://sfsfsdfdsfsdfdsdfssdf.se"
            };

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync((HttpRequestMessage request, CancellationToken token) =>
                {
                    HttpResponseMessage response = new HttpResponseMessage();
                    response.Content = new StringContent(JsonConvert.SerializeObject(new CrossbordpmtUpdate01Fcpsts01Response()
                    {
                        Result = new CrossbordpmtUpdate01Fcpsts01Tables()
                        {
                            Uakw4630 = new UAKW4630()
                            {
                                CreateTimestamp = "2020-02-12 11:38:00.000000",
                                Guid = "ced0f305-f722-4855-bfc7-5da3bf38bebc",
                                RowId = 1,
                                SourceId =  "TEST",
                                TransactionStatus = "REJECTED"
                            }
                        }
                    }));
                    return response;
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);

            _clientFactoryMock.Setup(a => a.CreateClient(It.IsAny<string>())).Returns(httpClient);
            _configMock.Setup(a => a.Value).Returns(_hubertClientOptions);
            _hubertClient = new HubertClient(_clientFactoryMock.Object, _configMock.Object, _loggerMock.Object);
        }

        [Fact]
        public void SendAssessmentResponse_WillCreateAndPostRequest()
        {
            const string timestamp = "";
            const string guid = "ced0f305-f722-4855-bfc7-5da3bf38bebc";
            const string transactionStatus = "";

            var response = _hubertClient.SendAssessmentResponse(timestamp, guid, transactionStatus).Result;

            Assert.Equal(response.Result.Uakw4630.Guid, guid);
        }
    }
}
