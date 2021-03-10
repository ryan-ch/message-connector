using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Threading;
using Testing.Common;
using XB.Kafka.Config;
using Xunit;

namespace XB.Kafka.Tests
{
    public class KafkaConsumerUnitTests
    {
        private readonly IKafkaConsumer _kafkaConsumer;
        private readonly Mock<IConsumer<string, string>> _consumer;
        private readonly Mock<ILogger<KafkaConsumer>> _loggerMock;

        public KafkaConsumerUnitTests()
        {
            _loggerMock = new Mock<ILogger<KafkaConsumer>>();
            _consumer = new Mock<IConsumer<string, string>>();
            _consumer.Setup(a => a.Consume(It.IsAny<CancellationToken>()))
                .Returns(new ConsumeResult<string, string> { Message = new Message<string, string> { Value = "Test Message" }, Partition = new Partition(0) });
            var configurationMock = new Mock<IOptions<KafkaConsumerConfig>>();

            _kafkaConsumer = new KafkaConsumer(configurationMock.Object, _loggerMock.Object, _consumer.Object);
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
        }

        [Fact]
        public void Consume_WillLogAndReturnConsumedMessage()
        {
            // Arrange
            const string messageText = "Test Message";

            // Act
            var result = _kafkaConsumer.Consume("Test Topic");

            // Assert
            Assert.Equal(messageText, result);
            _loggerMock.VerifyLoggerCall(LogLevel.Information, $"Consumed message '{messageText}'", Times.Once());
        }

        [Fact]
        public void Consume_WillLogExceptionConsumer()
        {
            // Arrange
            const string topic = "Test Topic";
            var exception = new Exception("Test Exception");
            _consumer.Setup(a => a.Consume(It.IsAny<CancellationToken>()))
                .Throws(exception);

            //Act
            _kafkaConsumer.Consume(topic);

            //Assert
            _loggerMock.VerifyLoggerCall(LogLevel.Error, $"Error when consuming topic: {topic}", Times.Once(), exception);
        }

        [Fact]
        public void Consume_WillLogExceptionAndCloseConsumer()
        {
            // Arrange
            const string topic = "Test Topic";
            var exception = new OperationCanceledException();
            _consumer.Setup(a => a.Consume(It.IsAny<CancellationToken>()))
                .Throws(exception);

            //Act
            _kafkaConsumer.Consume(topic);

            //Assert
            _loggerMock.VerifyLoggerCall(LogLevel.Error, "Operation Canceled", Times.Once());
            _consumer.Verify(a => a.Close(), Times.Once);
        }
    }
}
