using System;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
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

            _clientFactoryMock.Setup(a => a.CreateClient(It.IsAny<string>())).Returns(_httpClientMock.Object);
            _configMock.Setup(a => a.Value).Returns(_hubertClientOptions);

            _hubertClient = new HubertClient(_clientFactoryMock.Object, _configMock.Object, _loggerMock.Object);
        }

        [Fact]
        public void SendAssessmentResponse_()
        {
            //Arrange

            //Act

            //Assert
        }
    }
}
