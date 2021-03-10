using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using XB.Kafka.Config;

namespace XB.Kafka
{
    public class KafkaProducer : IKafkaProducer
    {
        private readonly string _topic;
        private readonly ILogger<KafkaProducer> _logger;
        private readonly IProducer<int, string> _kafkaProducer;

        public KafkaProducer(IOptions<KafkaProducerConfig> configuration, ILogger<KafkaProducer> logger, IProducer<int, string> producer = null)
        {
            _logger = logger;
            _topic = configuration.Value.Topic;
            _kafkaProducer = producer ?? new ProducerBuilder<int, string>(configuration.Value).Build();
        }

        public async Task Produce(string message)
        {
            try
            {
                var partition = new Partition(0);
                var kafkaTopic = new TopicPartition(_topic, partition);

                var kafkaMessage = new Message<int, string>
                {
                    Key = partition.Value,
                    Timestamp = Timestamp.Default,
                    Value = message
                };
                await _kafkaProducer.ProduceAsync(kafkaTopic, kafkaMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Couldn't send ProcessTrail with message: {message}", message);
            }
        }
    }
}
