using IBM.XMS;
using Microsoft.Extensions.Logging;
using Moq;
using XB.IBM.MQ.Config;
using XB.IBM.MQ.Implementations;
using Xunit;

namespace XB.IBM.MQ.Tests
{
    public class MqBaseUnitTests
    {
        private readonly Mock<IConnection> _connectionMock;
        private readonly Mock<ISession> _sessionMock;
        private readonly Mock<IConnectionFactory> _connectionFactoryMock;
        private readonly MqBase _mqBase;

        private readonly MqConfigurations _mqConfigurations = new MqConfigurations
        {
            MqChannel = "TestChannel",
            MqQueueManagerName = "QueueManagerTestName",
            MqQueueName = "QueueTestName",
            MqHostname = "TestHostname",
            MqPort = 1414,
            MqSslPath = "",
            MqKeyRepo = "TestKey",
            MqSslCipher = "TestSslCipher",
            MqUsername = "TestUsername",
            MqPassword = "TestPassword"
        };
        public MqBaseUnitTests()
        {
            var loggerMock = new Mock<ILogger<MqBase>>();
            _connectionMock = new Mock<IConnection>();
            _sessionMock = new Mock<ISession>();
            _connectionFactoryMock = TestHelper.GetMqConnectionFactoryMock(_connectionMock, _sessionMock);

            _mqBase = new MqBase(_mqConfigurations, loggerMock.Object, _connectionFactoryMock.Object);
        }

        [Fact]
        public void MqBase_WillConfigureTheFactoryCorrectly()
        {
            // Arrange
            // Act
            // Assert
            _connectionFactoryMock.Verify(a => a.SetStringProperty(XMSC.WMQ_SSL_CIPHER_SPEC, _mqConfigurations.MqSslCipher), Times.Once);
            _connectionFactoryMock.Verify(a => a.SetStringProperty(XMSC.WMQ_SSL_KEY_REPOSITORY, _mqConfigurations.MqKeyRepo), Times.Once);
            _connectionFactoryMock.Verify(a => a.SetStringProperty(XMSC.USERID, _mqConfigurations.MqUsername), Times.Once);
            _connectionFactoryMock.Verify(a => a.SetStringProperty(XMSC.PASSWORD, _mqConfigurations.MqPassword), Times.Once);
            _connectionFactoryMock.Verify(a => a.SetStringProperty(XMSC.WMQ_HOST_NAME, _mqConfigurations.MqHostname), Times.Once);
            _connectionFactoryMock.Verify(a => a.SetIntProperty(XMSC.WMQ_PORT, _mqConfigurations.MqPort), Times.Once);
            _connectionFactoryMock.Verify(a => a.SetStringProperty(XMSC.WMQ_CHANNEL, _mqConfigurations.MqChannel), Times.Once);
            _connectionFactoryMock.Verify(a => a.SetStringProperty(XMSC.WMQ_QUEUE_MANAGER, _mqConfigurations.MqQueueManagerName), Times.Once);
            _connectionFactoryMock.Verify(a => a.SetStringProperty(XMSC.WMQ_QUEUE_NAME, _mqConfigurations.MqQueueName), Times.Once);
            _connectionFactoryMock.Verify(a => a.SetIntProperty(XMSC.WMQ_CONNECTION_MODE, XMSC.WMQ_CM_CLIENT), Times.Once);
        }

        [Fact]
        public void MqBase_WillInitiateConnectionAndSession()
        {
            // Arrange
            // Act
            // Assert
            _connectionFactoryMock.Verify(a => a.CreateConnection(), Times.Once);
            _connectionMock.Verify(a => a.CreateSession(true, AcknowledgeMode.AutoAcknowledge), Times.Once);
            _sessionMock.Verify(a => a.CreateQueue(_mqConfigurations.MqQueueName), Times.Once);
            _connectionMock.Verify(a => a.Start(), Times.Once);
        }

        [Fact]
        public void Commit_WillCallSessionCommit()
        {
            // Arrange
            // Act
            _mqBase.Commit();

            // Assert
            _sessionMock.Verify(a => a.Commit(), Times.Once);
        }

        [Fact]
        public void Rollback_WillCallSessionRollback()
        {
            // Arrange
            // Act
            _mqBase.Rollback();

            // Assert
            _sessionMock.Verify(a => a.Rollback(), Times.Once);
        }
    }
}