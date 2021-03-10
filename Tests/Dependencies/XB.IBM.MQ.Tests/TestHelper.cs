using IBM.XMS;
using Moq;

namespace XB.IBM.MQ.Tests
{
    internal static class TestHelper
    {
        internal static Mock<IConnectionFactory> GetMqConnectionFactoryMock(Mock<IConnection> connectionMock, Mock<ISession> sessionMock)
        {
            var factory = new Mock<IConnectionFactory>();
            var destination = new Mock<IDestination>();

            factory.Setup(a => a.CreateConnection())
                .Returns(connectionMock.Object);
            connectionMock.Setup(a => a.CreateSession(true, AcknowledgeMode.AutoAcknowledge))
                .Returns(sessionMock.Object);
            sessionMock.Setup(a => a.CreateQueue(It.IsAny<string>()))
                .Returns(destination.Object);

            return factory;
        }
    }
}
