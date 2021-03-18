using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using SEB.SEBCS.RTM.v1.Client.Uakm463;
using SEB.SEBCS.RTM.v1.Client.Uakm463.Crossbordpmt.Update01.Fcpsts01;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using XB.Hubert.Config;
using XGalaxy.Common.TestHelpers;
using Xunit;

namespace XB.Hubert.Tests
{
    public class HubertClientTests
    {
        private readonly IHubertClient _hubertClient;
        private readonly Mock<HttpMessageHandler> _messageHandlerMock;
        private readonly HubertClientOptions _hubertClientOptions = new HubertClientOptions { Url = "https://dummyurl.se", ClientId = "Fake Id" };
        private readonly CrossbordpmtUpdate01Fcpsts01Response _hubertResponse = new CrossbordpmtUpdate01Fcpsts01Response
        {
            Result = new CrossbordpmtUpdate01Fcpsts01Tables
            {
                Uakw4630 = new UAKW4630
                {
                    CreateTimestamp = "2020-02-12 11:38:00.000000",
                    Guid = "ced0f305-f722-4855-bfc7-5da3bf38bebc",
                    RowId = 1,
                    SourceId = "TEST",
                    TransactionStatus = "REJECTED"
                }
            }
        };

        public HubertClientTests()
        {
            var configMock = new Mock<IOptions<HubertClientOptions>>();
            configMock.Setup(a => a.Value).Returns(_hubertClientOptions);

            Mock<IHttpClientFactory> clientFactoryMock;
            (clientFactoryMock, _messageHandlerMock) = TestUtilities.GetHttpClientFactoryMock(JsonConvert.SerializeObject(_hubertResponse));
            _hubertClient = new HubertClient(clientFactoryMock.Object, configMock.Object);
        }

        [Fact]
        public async Task SendAssessmentResultAsync_WillPassCorrectParameters()
        {
            // Arrange
            const string id = "Random Id";
            const string transactionStatus = "Some Status";
            var timestamp = DateTime.Now;

            // Act
            _ = await _hubertClient.SendAssessmentResultAsync(timestamp, id, transactionStatus);

            // Assert
            var match = new Func<HttpRequestMessage, bool>(h =>
              h.Headers.FirstOrDefault(a => a.Key == "x-sebcs-clientid").Value.First() == _hubertClientOptions.ClientId &&
              h.RequestUri.AbsoluteUri.StartsWith(_hubertClientOptions.Url));

            _messageHandlerMock.Protected().Verify("SendAsync", Times.Once(),
                ItExpr.Is<HttpRequestMessage>(h => match(h)), ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task SendAssessmentResultAsync_WillReturnCorrectResult()
        {
            // Arrange
            const string id = "Random Id";
            const string transactionStatus = "Some Status";
            var timestamp = DateTime.Now;

            // Act
            var response = await _hubertClient.SendAssessmentResultAsync(timestamp, id, transactionStatus);

            // Assert
            Assert.Equal(JsonConvert.SerializeObject(_hubertResponse), JsonConvert.SerializeObject(response));
        }
    }
}
