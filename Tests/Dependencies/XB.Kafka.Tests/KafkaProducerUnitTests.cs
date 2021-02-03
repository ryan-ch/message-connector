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
    public class KafkaProducerUnitTests
    {
        private readonly IKafkaProducer _kafkaProducer;
        private readonly KafkaProducerConfig _producerConfig;
        private readonly Mock<IProducer<int, string>> _producer;
        private readonly Mock<ILogger<KafkaProducer>> _loggerMock;
        private readonly Mock<IOptions<KafkaProducerConfig>> _configurationMock;

        public KafkaProducerUnitTests()
        {
            _loggerMock = new Mock<ILogger<KafkaProducer>>();
            _producer = new Mock<IProducer<int, string>>();

            _producerConfig = new KafkaProducerConfig { Topic = "TestTopic001" };
            _configurationMock = new Mock<IOptions<KafkaProducerConfig>>();
            _configurationMock.Setup(a => a.Value).Returns(_producerConfig);

            _kafkaProducer = new KafkaProducer(_configurationMock.Object, _loggerMock.Object, _producer.Object);
        }

        [Fact]
        public void Produce_WillProduceTheMessageWithProvidedData()
        {
            // Arrange
            const string kafkaMessage = "Test Kafka Message";
            TopicPartition passedTopic = null;
            Message<int, string> passedMessage = null;
            _producer.Setup(a => a.ProduceAsync(It.IsAny<TopicPartition>(), It.IsAny<Message<int, string>>(), default))
                .Callback<TopicPartition, Message<int, string>, CancellationToken>((a, b, c) =>
                  {
                      passedTopic = a;
                      passedMessage = b;
                  });

            //Act
            _kafkaProducer.Produce(kafkaMessage);

            // Assert Topic
            Assert.Equal(_producerConfig.Topic, passedTopic?.Topic);
            Assert.Equal(0, passedTopic?.Partition.Value);

            // Assert Message
            Assert.Equal(0, passedMessage?.Key);
            Assert.Equal(kafkaMessage, passedMessage?.Value);
            Assert.Equal(Timestamp.Default, passedMessage?.Timestamp);
        }

        [Fact]
        public void Produce_WhenExceptionIsThrownItWillBeLogged()
        {
            //Arrange
            var exception = new Exception("Test exception message");
            _producer.Setup(a => a.ProduceAsync(It.IsAny<TopicPartition>(), It.IsAny<Message<int, string>>(), default))
                .Throws(exception);

            //Act
            _kafkaProducer.Produce("Test Message");

            //Assert
            _loggerMock.Verify(
                m => m.Log(
                   LogLevel.Error,
                   It.IsAny<EventId>(),
                   It.Is<It.IsAnyType>((v, _) => v.ToString().Contains("Couldn't send ProcessTrail with message: Test Message")),
                   exception,
                   It.IsAny<Func<It.IsAnyType, Exception, string>>()),
               Times.Once);
        }
    }
}
