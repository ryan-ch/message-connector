using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
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
using XGalaxy.Common.TestHelpers;
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
        public async Task SendAsync_ShouldInjectJwtAndDoRequest()
        {
            //Arrange
            var mainResponse = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("Result") };
            var innerMock = new Mock<DelegatingHandler>();

            innerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", _sebcsRequestMatcher, ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(mainResponse);

            innerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", _jwtRequestMatcher, ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("{ \"access_token\": \"bearertoken123\"}") });

            var authenticationDelegatingHandler = new AuthenticationDelegatingHandler(_configurationMock.Object, NullLogger<AuthenticationDelegatingHandler>.Instance)
            {
                InnerHandler = innerMock.Object
            };
            var request = new HttpRequestMessage(HttpMethod.Post, SebCsUrl);
            var invoker = new HttpMessageInvoker(authenticationDelegatingHandler);

            //Act
            var res = await invoker.SendAsync(request, CancellationToken.None);

            //Assert
            Assert.Equal("Bearer bearertoken123", request.Headers.Authorization.ToString());
            Assert.Equal(mainResponse, res);
        }

        [Fact]
        public async Task SendAsync_ShouldReuseJwtAndDoRequest()
        {
            //Arrange
            var mainResponse1 = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("Result 1") };
            var mainResponse2 = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("Result 2") };
            var innerMock = new Mock<DelegatingHandler>();

            innerMock.Protected()
                .SetupSequence<Task<HttpResponseMessage>>("SendAsync", _sebcsRequestMatcher, ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(mainResponse1)
                .ReturnsAsync(mainResponse2);

            innerMock.Protected()
                .SetupSequence<Task<HttpResponseMessage>>("SendAsync", _jwtRequestMatcher, ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("{ \"access_token\": \"bearertoken1\", \"expires_in\":\"300\"}") })
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("{ \"access_token\": \"bearertoken2\", \"expires_in\":\"300\"}") });

            var authenticationDelegatingHandler = new AuthenticationDelegatingHandler(_configurationMock.Object, NullLogger<AuthenticationDelegatingHandler>.Instance)
            {
                InnerHandler = innerMock.Object
            };

            var request = new HttpRequestMessage(HttpMethod.Post, SebCsUrl);
            var request2 = new HttpRequestMessage(HttpMethod.Post, SebCsUrl);

            var invoker = new HttpMessageInvoker(authenticationDelegatingHandler);

            //Act
            var result1 = await invoker.SendAsync(request, CancellationToken.None);
            var result2 = await invoker.SendAsync(request2, CancellationToken.None);

            //Assert
            Assert.Equal("Bearer bearertoken1", request.Headers.Authorization.ToString());
            Assert.Equal("Bearer bearertoken1", request2.Headers.Authorization.ToString());

            Assert.Equal(mainResponse1, result1);
            Assert.Equal(mainResponse2, result2);
        }

        [Fact]
        public async Task SendAsync_ShouldFetchNewTokenOnUnauthorized()
        {
            //Arrange
            var innerMock = new Mock<DelegatingHandler>();

            innerMock.Protected()
                .SetupSequence<Task<HttpResponseMessage>>("SendAsync", _sebcsRequestMatcher, ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.Unauthorized))
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

            innerMock.Protected()
                .SetupSequence<Task<HttpResponseMessage>>("SendAsync", _jwtRequestMatcher, ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("{ \"access_token\": \"bearertoken1\"}") })
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("{ \"access_token\": \"bearertoken2\"}") });

            var authenticationDelegatingHandler = new AuthenticationDelegatingHandler(_configurationMock.Object, NullLogger<AuthenticationDelegatingHandler>.Instance)
            {
                InnerHandler = innerMock.Object
            };
            var request = new HttpRequestMessage(HttpMethod.Post, SebCsUrl);
            var invoker = new HttpMessageInvoker(authenticationDelegatingHandler);

            //Act
            var result = await invoker.SendAsync(request, CancellationToken.None);

            //Assert
            Assert.Equal($"Bearer bearertoken2", request.Headers.Authorization.ToString());
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async Task SendAsync_IfFailedToGetJWT_ShouldLogErrorAndThrowIt()
        {
            //Arrange
            var logMock = new Mock<ILogger<AuthenticationDelegatingHandler>>();
            var exception = new Exception("Error Can't Generate JWT");
            var innerMock = new Mock<DelegatingHandler>();
            innerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", _jwtRequestMatcher, ItExpr.IsAny<CancellationToken>())
                .Throws(exception);
            var authenticationDelegatingHandler = new AuthenticationDelegatingHandler(_configurationMock.Object, logMock.Object)
            {
                InnerHandler = innerMock.Object
            };
            var request = new HttpRequestMessage(HttpMethod.Post, SebCsUrl);
            var invoker = new HttpMessageInvoker(authenticationDelegatingHandler);

            //Act
            await Assert.ThrowsAsync<Exception>(() => invoker.SendAsync(request, CancellationToken.None));

            // Assert
            logMock.VerifyLoggerCall(LogLevel.Error, "Unable to generate JWT for requests", Times.Once(), exception);
        }
    }
}
