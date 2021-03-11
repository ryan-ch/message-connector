using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Testing.Common
{
    public static class TestUtilities
    {
        public static (Mock<IHttpClientFactory>, Mock<HttpMessageHandler>) GetHttpClientFactoryMock(string expectedResultString, string baseUri = null, HttpStatusCode status = HttpStatusCode.OK)
        {
            var factoryMock = new Mock<IHttpClientFactory>();
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = status,
                    Content = new StringContent(expectedResultString)
                });

            factoryMock.Setup(a => a.CreateClient(It.IsAny<string>()))
                .Returns(new HttpClient(mockHttpMessageHandler.Object) { BaseAddress = baseUri == null ? null : new Uri(baseUri) });

            return (factoryMock, mockHttpMessageHandler);
        }

        public static void VerifyLoggerCall<T>(this Mock<ILogger<T>> loggerMock, LogLevel level, string message, Times times, Exception exception = null)
        {
            if (exception == null)
                loggerMock.Verify(a => a.Log(
                        level,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((v, _) => v.ToString().Contains(message)),
                        It.IsAny<Exception>(),
                        It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                    times);
            else
                loggerMock.Verify(a => a.Log(
                        level,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((v, _) => v.ToString().Contains(message)),
                        exception,
                        It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                    times);
        }
    }
}
