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
        private readonly ILogger<KafkaProducer> _logger;
        public IProducer<int, string> kafkaProducer { get; set; }
        public string Topic { get; }


        public KafkaProducer(IOptions<KafkaProducerConfig> configuration, ILogger<KafkaProducer> logger)
        {
            _logger = logger;
            Topic = configuration.Value.Topic;
            kafkaProducer = new ProducerBuilder<int, string>(configuration.Value).Build();
        }

        public async Task Execute(string message)
        {
            try
            {
                var partition = new Partition(0);
                var kafkaTopic = new TopicPartition(Topic, partition);

                var kafkaMessage = new Message<int, string>
                {
                    Key = partition.Value,
                    Timestamp = Timestamp.Default,
                    Value = message
                };
                await kafkaProducer.ProduceAsync(kafkaTopic, kafkaMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Couldn't send ProcessTrail with message: " + message);
            }
        }
    }
}
