using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using XB.HttpClientJwt.Config;
using Xunit;

namespace XB.HttpClientJwt.Tests
{
    public class HttpClientJwtTests
    {
        private readonly HttpClientJwtOptions _httpClientJwtOptions;
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly AuthenticationDelegatingHandler _authenticationDelegatingHandler;

        public HttpClientJwtTests()
        {
            var loggerMock = new Mock<ILogger<AuthenticationDelegatingHandler>>();

            _httpClientJwtOptions = new HttpClientJwtOptions()
            {
                Grant_Type = "Grant_Type",
                Scope = "Scope",
                ClientId = "ClientId",
                ClientSecret = "ClientSecret",
                Password = "Password",
                Url = "http://test.com",
                Username = "Username"
            };
            var configurationMock = new Mock<IOptions<HttpClientJwtOptions>>();
            configurationMock.Setup(a => a.Value).Returns(_httpClientJwtOptions);

            _authenticationDelegatingHandler = new AuthenticationDelegatingHandler(loggerMock.Object, configurationMock.Object)
            {
                InnerHandler = new TestHandler((r, c) =>
                {
                    return TestHandler.Return200();
                })
            };
        }

        [Fact]
        public void Test1()
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, _httpClientJwtOptions.Url);

            var client = new HttpClient(_authenticationDelegatingHandler);
            var result = client.SendAsync(httpRequestMessage).Result;

            Assert.True(result.StatusCode == HttpStatusCode.OK);
        }
    }

    public class TestHandler : DelegatingHandler
    {
        private readonly Func<HttpRequestMessage,
            CancellationToken, Task<HttpResponseMessage>> _handlerFunc;

        public TestHandler()
        {
            _handlerFunc = (r, c) => Return200();
        }

        public TestHandler(Func<HttpRequestMessage,
            CancellationToken, Task<HttpResponseMessage>> handlerFunc)
        {
            _handlerFunc = handlerFunc;
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return _handlerFunc(request, cancellationToken);
        }

        public static Task<HttpResponseMessage> Return200()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent("{\"access_token\":\"access_token\", \"expires_in\":\"300\"}");
            return Task.Factory.StartNew(
                () => response);
        }
    }
}
