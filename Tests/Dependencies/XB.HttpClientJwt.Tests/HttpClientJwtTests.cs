using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using XB.HttpClientJwt.Config;
using Xunit;

namespace XB.HttpClientJwt.Tests
{
    public class HttpClientJwtTests
    {
        private const string SebCsUrl = "https://localhost/sebcs";
        private const string JwtUrl = "https://localhost/jwt";

        private readonly Mock<IOptions<HttpClientJwtOptions>> _configurationMock;

        private readonly Expression _sebcsRequestMatcher = ItExpr.Is((HttpRequestMessage request) => request.RequestUri == new Uri(SebCsUrl));
        private readonly Expression _jwtRequestMatcher = ItExpr.Is((HttpRequestMessage request) => request.RequestUri == new Uri(JwtUrl));


        public HttpClientJwtTests()
        {
            var httpClientJwtOptions = new HttpClientJwtOptions()
            {
                Grant_Type = "Grant_Type",
                Scope = "Scope",
                ClientId = "ClientId",
                ClientSecret = "ClientSecret",
                Password = "Password",
                Url = JwtUrl,
                Username = "Username"
            };
            _configurationMock = new Mock<IOptions<HttpClientJwtOptions>>();
            _configurationMock.Setup(a => a.Value).Returns(httpClientJwtOptions);
        }

        [Fact]
        public async Task FetchJwtToken_ShouldReturnJwtAndDoRequest()
        {
            //Arrange
            var innerMock = new Mock<DelegatingHandler>();

            innerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", _sebcsRequestMatcher,
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)).Verifiable();

            innerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", _jwtRequestMatcher,
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
                { Content = new StringContent("{ \"access_token\": \"bearertoken123\"}") }).Verifiable();

            var authenticationDelegatingHandler = SetupDelegatingHandler(innerMock.Object);
            var request = new HttpRequestMessage(HttpMethod.Post, SebCsUrl);
            var invoker = new HttpMessageInvoker(authenticationDelegatingHandler);

            //Act
            await invoker.SendAsync(request, CancellationToken.None);

            //Assert
            Assert.Equal($"Bearer bearertoken123", request.Headers.Authorization.ToString());
        }

        [Fact]
        public async Task FetchJwtToken_ShouldReuseJwtAndDoRequest()
        {
            //Arrange
            var innerMock = new Mock<DelegatingHandler>();

            innerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", _sebcsRequestMatcher, ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

            innerMock.Protected()
                .SetupSequence<Task<HttpResponseMessage>>("SendAsync", _jwtRequestMatcher, ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
                { Content = new StringContent("{ \"access_token\": \"bearertoken1\", \"expires_in\":\"300\"}") })
                .ThrowsAsync(new Exception("tried to fetch new query"));

            var authenticationDelegatingHandler = SetupDelegatingHandler(innerMock.Object);

            var request = new HttpRequestMessage(HttpMethod.Post, SebCsUrl);
            var request2 = new HttpRequestMessage(HttpMethod.Post, SebCsUrl);

            var invoker = new HttpMessageInvoker(authenticationDelegatingHandler);

            //Act
            var result = await invoker.SendAsync(request, CancellationToken.None);
            var result2 = await invoker.SendAsync(request2, CancellationToken.None);

            //Assert
            Assert.Equal($"Bearer bearertoken1", request.Headers.Authorization.ToString());
            Assert.Equal($"Bearer bearertoken1", request2.Headers.Authorization.ToString());

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(HttpStatusCode.OK, result2.StatusCode);
        }

        [Fact]
        public async Task FetchJwtToken_ShouldFetchNewTokenOnUnauthorized()
        {
            //Arrange
            var innerMock = new Mock<DelegatingHandler>();

            innerMock.Protected()
                .SetupSequence<Task<HttpResponseMessage>>("SendAsync", _sebcsRequestMatcher, ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.Unauthorized))
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

            innerMock.Protected()
                .SetupSequence<Task<HttpResponseMessage>>("SendAsync", _jwtRequestMatcher, ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
                { Content = new StringContent("{ \"access_token\": \"bearertoken1\"}") })
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
                { Content = new StringContent("{ \"access_token\": \"bearertoken2\"}") });

            var authenticationDelegatingHandler = SetupDelegatingHandler(innerMock.Object);
            var request = new HttpRequestMessage(HttpMethod.Post, SebCsUrl);
            var invoker = new HttpMessageInvoker(authenticationDelegatingHandler);

            //Act
            var result = await invoker.SendAsync(request, CancellationToken.None);

            //Assert
            Assert.Equal($"Bearer bearertoken2", request.Headers.Authorization.ToString());
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }

        private AuthenticationDelegatingHandler SetupDelegatingHandler(DelegatingHandler handler)
        {
            return new AuthenticationDelegatingHandler(_configurationMock.Object)
            {
                InnerHandler = handler
            };
        }
    }
}
