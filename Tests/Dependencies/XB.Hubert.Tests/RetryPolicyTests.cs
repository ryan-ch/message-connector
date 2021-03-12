using Moq;
using Moq.Protected;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Testing.Common;
using Xunit;

namespace XB.Hubert.Tests
{
    public class RetryPolicyTests
    {
        [Theory]
        [InlineData(1,1)]
        [InlineData(2,0)]
        public async Task RetryPolicy_ShouldRetryCalls_WithAttemptsAndDelayBasedOnConfiguration(int retryAttempts, double retrySleepDuration)
        {
            // Arrange
            var policy = HubertExtensions.GetRetryPolicy(retryAttempts, retrySleepDuration);
            (var client, var messageHandlerMock) = GetHttpClientMock(HttpStatusCode.RequestTimeout);

            // Act
            await policy.ExecuteAsync(() => client.SendAsync(new HttpRequestMessage()));

            // Assert
            messageHandlerMock.Protected().Verify("SendAsync", Times.Exactly(1 + retryAttempts), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task RetryPolicy_ShouldNotRetry_OnSubsequentSuccess()
        {
            // Arrange
            var policy = HubertExtensions.GetRetryPolicy(5, 0);
            (var client, var messageHandlerMock) = GetHttpClientMock(HttpStatusCode.NotFound);
            messageHandlerMock.Protected().SetupSequence<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.RequestTimeout,
                    Content = null
                })
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = null
                });

            // Act
            await policy.ExecuteAsync(() => client.SendAsync(new HttpRequestMessage()));

            // Assert
            messageHandlerMock.Protected().Verify("SendAsync", Times.Exactly(2), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task RetryPolicy_ShouldRetryCalls_OnHttpExceptions()
        {
            // Arrange
            var policy = HubertExtensions.GetRetryPolicy(1, 0);
            (var client, var messageHandlerMock) = GetHttpClientMock(HttpStatusCode.NotFound);
            messageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException());

            // Act
            _ = Assert.ThrowsAnyAsync<HttpRequestException>(async () => await policy.ExecuteAsync(() => client.SendAsync(new HttpRequestMessage())));

            // Assert
            messageHandlerMock.Protected().Verify("SendAsync", Times.Exactly(2), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
        }

        [Theory]
        [InlineData(HttpStatusCode.RequestTimeout)]
        [InlineData(HttpStatusCode.InternalServerError)]
        [InlineData(HttpStatusCode.BadGateway)]
        [InlineData(HttpStatusCode.ServiceUnavailable)]
        [InlineData(HttpStatusCode.GatewayTimeout)]
        [InlineData(HttpStatusCode.NetworkAuthenticationRequired)]
        public async Task RetryPolicy_ShouldRetryCalls_ForExpectedHttpStatusCodes(HttpStatusCode httpStatusCode)
        {
            // Arrange
            var policy = HubertExtensions.GetRetryPolicy(1, 0);
            (var client, var messageHandlerMock) = GetHttpClientMock(httpStatusCode);

            // Act
            await policy.ExecuteAsync(() => client.SendAsync(new HttpRequestMessage()));

            // Assert
            messageHandlerMock.Protected().Verify("SendAsync", Times.Exactly(2), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
        }

        [Theory]
        [InlineData(HttpStatusCode.NotFound)]
        [InlineData(HttpStatusCode.OK)]
        [InlineData(HttpStatusCode.Unauthorized)]
        [InlineData(HttpStatusCode.Forbidden)]
        public async Task RetryPolicy_ShouldNotRetryCalls_ForUncoveredHttpStatusCodes(HttpStatusCode httpStatusCode)
        {
            // Arrange
            var policy = HubertExtensions.GetRetryPolicy(1, 0);
            (var client, var messageHandlerMock) = GetHttpClientMock(httpStatusCode);

            // Act
            await policy.ExecuteAsync(() => client.SendAsync(new HttpRequestMessage()));

            // Assert
            messageHandlerMock.Protected().Verify("SendAsync", Times.Once(), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
        }

        private (HttpClient client, Mock<HttpMessageHandler> messageHandler) GetHttpClientMock(HttpStatusCode httpStatusCode)
        {
            (var clientFactoryMock, var messageHandlerMock) = TestUtilities.GetHttpClientFactoryMock(string.Empty, "http://dummyUrl.com/api", httpStatusCode);
            return (clientFactoryMock.Object.CreateClient(), messageHandlerMock);
        }
    }
}
