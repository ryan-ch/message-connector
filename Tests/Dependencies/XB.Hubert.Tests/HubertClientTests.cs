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
        //private readonly HttpClient _httpClient;
        private readonly Mock<ILogger<HubertClient>> _loggerMock;
        private readonly Mock<IHttpClientFactory> _clientFactoryMock;
        private readonly IHubertClient _hubertClient;

        public HubertClientTests()
        {
            _configMock = new Mock<IOptions<HubertClientOptions>>();
            _loggerMock = new Mock<ILogger<HubertClient>>();
            _clientFactoryMock = new Mock<IHttpClientFactory>();

            _hubertClient = new HubertClient(_clientFactoryMock.Object, _configMock.Object, _loggerMock.Object);
        }

        [Fact]
        public void SendAssessmentResponse_()
        {

        }
    }
}
