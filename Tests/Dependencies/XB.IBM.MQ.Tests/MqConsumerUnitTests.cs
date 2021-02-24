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
        private readonly Mock<IMessageConsumer> _messageConsumerMock;
        private readonly Mock<ITextMessage> _textMessage;

        public MqConsumerUnitTests()
        {
            var configurationMock = new Mock<IOptions<MqOptions>>();
            var connectionMock = new Mock<IConnection>();
            var loggerMock = new Mock<ILogger<MqConsumer>>();
            var sessionMock = new Mock<ISession>();
            _messageConsumerMock = new Mock<IMessageConsumer>();
            _textMessage = new Mock<ITextMessage>();

            var mqOptions = new MqOptions { ReaderConfig = new MqConfigurations() };

            _messageConsumerMock.Setup(a => a.Receive(0)).Returns(_textMessage.Object);
            sessionMock.Setup(a => a.CreateConsumer(It.IsAny<IDestination>())).Returns(_messageConsumerMock.Object);

            configurationMock.Setup(ap => ap.Value).Returns(mqOptions);
            var connectionFactoryMock = TestHelper.GetConnectionFactoryMock(connectionMock, sessionMock);
            _mqConsumer = new MqConsumer(configurationMock.Object, loggerMock.Object, connectionFactoryMock.Object);
        }

        [Fact]
        public void ReceiveMessage_WillReturnCorrectMessageForConsumer()
        {
            //Arrange
            const string txtString = "TEST";
            _textMessage.Setup(a => a.Text).Returns(txtString);

            //Act
            var result = _mqConsumer.ReceiveMessage(0);

            //Assert
            _messageConsumerMock.Verify(a => a.Receive(0), Times.Once);
            Assert.Equal(txtString, result);
        }
        [Fact]
        public void ReceiveMessage_WillReceiveNullAsMessageForConsumer()
        {
            //Arrange
            const string nullString = null;
            _textMessage.Setup(a => a.Text).Returns(nullString);
            //Act
            var result = _mqConsumer.ReceiveMessage(0);
            //Assert
            Assert.Null(result);
        }
    }
}