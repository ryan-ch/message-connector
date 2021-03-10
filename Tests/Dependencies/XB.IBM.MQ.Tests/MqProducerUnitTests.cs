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
        private readonly Mock<ISession> _sessionMock;
        private readonly Mock<IMessageProducer> _messageProducerMock;
        private readonly Mock<ITextMessage> _textMessage;

        public MqProducerUnitTests()
        {
            var loggerMock = new Mock<ILogger<MqProducer>>();
            var connectionMock = new Mock<IConnection>();
            _sessionMock = new Mock<ISession>();
            _messageProducerMock = new Mock<IMessageProducer>();
            var configurationsMock = new Mock<IOptions<MqOptions>>();
            _textMessage = new Mock<ITextMessage>();

            var mqOptions = new MqOptions { WriterConfig = new MqConfigurations() };

            configurationsMock.Setup(a => a.Value).Returns(mqOptions);
            var connectionFactoryMock = TestHelper.GetMqConnectionFactoryMock(connectionMock, _sessionMock);
            _sessionMock.Setup(s => s.CreateProducer(It.IsAny<IDestination>())).Returns(_messageProducerMock.Object);
            _sessionMock.Setup(s => s.CreateTextMessage(It.IsAny<string>())).Returns(_textMessage.Object);

            _mqProducer = new MqProducer(configurationsMock.Object, loggerMock.Object, connectionFactoryMock.Object);
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
            const string message = "testing testing";

            //Act
            _mqProducer.WriteMessage(message);

            //Assert
            _messageProducerMock.Verify(a => a.Send(_textMessage.Object), Times.Once);
        }
    }
}