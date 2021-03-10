using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using XB.Kafka.Config;

namespace XB.Kafka
{
    public class KafkaConsumer : IKafkaConsumer
    {
        private readonly ILogger<KafkaConsumer> _logger;
        private readonly IConsumer<string, string> _kafkaConsumer;

        public KafkaConsumer(IOptions<KafkaConsumerConfig> configuration, ILogger<KafkaConsumer> logger, IConsumer<string, string> consumer = null)
        {
            _logger = logger;
            _kafkaConsumer = consumer ?? new ConsumerBuilder<string, string>(configuration.Value).Build();
        }

        public string Consume(string topic)
        {
            var message = "";
            try
            {
                _kafkaConsumer.Subscribe(topic);
                var result = _kafkaConsumer.Consume();
                _logger.LogInformation($"Consumed message '{result.Message.Value}' at: '{result.TopicPartitionOffset}'.");
                // Todo: Should we return the message or the whole result?
                message = result.Message.Value;
            }
            catch (OperationCanceledException)
            {
                _logger.LogError("Operation Canceled");
                _kafkaConsumer.Close();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when consuming topic: {topic}", topic);
            }
            return message;
        }
    }
}
