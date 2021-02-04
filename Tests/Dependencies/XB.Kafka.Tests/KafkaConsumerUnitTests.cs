using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Threading;
using XB.Kafka.Config;
using Xunit;

namespace XB.Kafka.Tests
{
    public class KafkaConsumerUnitTests
    {
        private readonly IKafkaConsumer _kafkaConsumer;
        private readonly Mock<IConsumer<string, string>> _consumer;
        private readonly Mock<ILogger<KafkaConsumer>> _loggerMock;
        private readonly Mock<IOptions<KafkaConsumerConfig>> _configurationMock;

        public KafkaConsumerUnitTests()
        {
            _loggerMock = new Mock<ILogger<KafkaConsumer>>();
            _consumer = new Mock<IConsumer<string, string>>();
            _configurationMock = new Mock<IOptions<KafkaConsumerConfig>>();

            _kafkaConsumer = new KafkaConsumer(_configurationMock.Object, _loggerMock.Object, _consumer.Object);
        }

        [Fact]
        public void Consume_WillSubscribeToConsumerAndCloseIt()
        {
            // Arrange
            const string topic = "Test Topic";

            // Act
            _kafkaConsumer.Consume(topic);

            // Assert
            _consumer.Verify(a => a.Subscribe(topic), Times.Once);
            _consumer.Verify(a => a.Close(), Times.Once);
        }

        [Fact]
        public void Consume_WillLogAndReturnConsumedMessage()
        {
            // Arrange
            const string messageText = "Test Message";
            _consumer.Setup(a => a.Consume(It.IsAny<CancellationToken>()))
                .Returns(new ConsumeResult<string, string> { Message = new Message<string, string> { Value = messageText }, Partition = new Partition(0) });

            // Act
            var result = _kafkaConsumer.Consume("Test Topic");

            // Assert
            Assert.Equal(messageText, result);
            _loggerMock.Verify(a => a.Log(
                  LogLevel.Information,
                  It.IsAny<EventId>(),
                  It.Is<It.IsAnyType>((v, _) => v.ToString().Contains($"Consumed message '{messageText}'")),
                  null,
                  It.IsAny<Func<It.IsAnyType, Exception, string>>()),
              Times.Once);
        }

        [Fact]
        public void Consume_WillLogExceptionAndCloseConsumer()
        {
            // Arrange
            const string topic = "Test Topic";
            var exception = new Exception("Test Exception");
            _consumer.Setup(a => a.Consume(It.IsAny<CancellationToken>()))
                .Throws(exception);

            //Act
            _kafkaConsumer.Consume(topic);

            //Assert
            _loggerMock.Verify(a => a.Log(
                  LogLevel.Error,
                  It.IsAny<EventId>(),
                  It.Is<It.IsAnyType>((v, _) => v.ToString().Contains($"Error when consuming topic: " + topic)),
                  exception,
                  It.IsAny<Func<It.IsAnyType, Exception, string>>()),
              Times.Once);
            _consumer.Verify(a => a.Close(), Times.Once);
        }
    }
}
