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
    public class MqConsumerUnitTests
    {
        private readonly IMqConsumer _mqConsumer;
        private readonly Mock<ILogger<MqConsumer>> _loggerMock;
        private readonly Mock<ISession> _sessionMock;
        private readonly Mock<IMessageConsumer> _messageConsumerMock;
        private readonly Mock<ITextMessage> _textMessage;

        public MqConsumerUnitTests()
        {
            Mock<IOptions<MqOptions>> _configurationMock = new Mock<IOptions<MqOptions>>();
            Mock<IConnection> _connectionMock = new Mock<IConnection>();
            _loggerMock = new Mock<ILogger<MqConsumer>>();
            _sessionMock = new Mock<ISession>();
            _messageConsumerMock = new Mock<IMessageConsumer>();
            _textMessage = new Mock<ITextMessage>();

            MqOptions _mqOptions = new MqOptions()
            {
                ReaderConfig = new MqConfigurations()
            };
            
            _messageConsumerMock.Setup(a => a.Receive(0)).Returns(_textMessage.Object);
            _sessionMock.Setup(a => a.CreateConsumer(It.IsAny<IDestination>())).Returns(_messageConsumerMock.Object);
            
            _configurationMock.Setup(ap => ap.Value).Returns(_mqOptions);
            Mock<IConnectionFactory> _connectionFactoryMock = TestHelper.GetConnectionFactoryMock(_connectionMock, _sessionMock);
            _mqConsumer = new MqConsumer(_configurationMock.Object, _loggerMock.Object, _connectionFactoryMock.Object);
        }

        [Fact]
        public void ReceiveMessage_WillReturnCorrectMessageForConsumer()
        {
            //Arrange
            const string txtString = "TEST";
            _textMessage.Setup(a => a.Text).Returns(txtString);

            //Act
            string result = _mqConsumer.ReceiveMessage(0);

            //Assert
            _messageConsumerMock.Verify(a => a.Receive(0), Times.Once);
            Assert.Equal(txtString, result);
        }
        [Fact]
        public void ReceiveMessege_WillReceiveNullAsMessegeForConsumer()
        {
            //Arrange
            const string nullString = null;
            _textMessage.Setup(a => a.Text).Returns(nullString);
            //Act
            string result = _mqConsumer.ReceiveMessage(0);
            //Assert
            Assert.Null(result);
        }
    }
}