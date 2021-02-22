using System;
using IBM.XMS;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using XB.IBM.MQ.Config;
using XB.IBM.MQ.Implementations;
using XB.IBM.MQ.Interfaces;
using Xunit;

namespace XB.IBM.MQ.Tests
{
    public class MqProducerUnitTests
    {
        private readonly IMqProducer _mqProducer;
        private readonly Mock<ILogger<MqProducer>> _loggerMock;
        private readonly Mock<ISession> _sessionMock;
        private readonly Mock<IMessageProducer> _messageProducerMock;
        private readonly Mock<ITextMessage> _textMessage;

        public MqProducerUnitTests()
        {
            _loggerMock = new Mock<ILogger<MqProducer>>();
            Mock<IConnection> _connectionMock = new Mock<IConnection>();
            _sessionMock = new Mock<ISession>();
            _messageProducerMock = new Mock<IMessageProducer>();
            Mock<IOptions<MqOptions>> _configurationsMock = new Mock<IOptions<MqOptions>>();
            _textMessage = new Mock<ITextMessage>();

            MqOptions _mqOptions = new MqOptions()
            {
                WriterConfig = new MqConfigurations()
            };

            _configurationsMock.Setup(a => a.Value).Returns(_mqOptions);
            Mock<IConnectionFactory> _connectionFactoryMock = TestHelper.GetConnectionFactoryMock(_connectionMock, _sessionMock);
            _sessionMock.Setup(s => s.CreateProducer(It.IsAny<IDestination>())).Returns(_messageProducerMock.Object);
            _sessionMock.Setup(s => s.CreateTextMessage(It.IsAny<string>())).Returns(_textMessage.Object);

            _mqProducer = new MqProducer(_configurationsMock.Object, _loggerMock.Object, _connectionFactoryMock.Object);
        }

        [Fact]
        public void WriteMessage_SessionCreateNewMessageIsCalled()
        {
            //Arrange
            const string message = "testing testing";

            //Act
            _mqProducer.WriteMessage(message);

            //Assert
            _sessionMock.Verify(a => a.CreateTextMessage(message), Times.Once);
        }

        [Fact]
        public void WriteMessage_ProducerSendsMessage()
        {
            //Arrange
            string message = "testing testing";

            //Act
            _mqProducer.WriteMessage(message);

            //Assert
            _messageProducerMock.Verify(a => a.Send(_textMessage.Object), Times.Once);
        }
    }
}