using Confluent.Kafka;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using XB.Kafka.Config;

namespace XB.Kafka
{
    public class KafkaProducer : IKafkaProducer
    {
        public IProducer<int, string> kafkaProducer { get; set; }
        public string Topic { get; }

        public KafkaProducer(IOptions<KafkaConfig> configuration)
        {
            Topic = configuration.Value.Topic;
            kafkaProducer = new ProducerBuilder<int, string>(configuration.Value).Build();
        }

        public Task Execute(string message)
        {
            var partition = new Partition(0);
            var kafkaTopic = new TopicPartition(Topic, partition);

            var kafkaMessage = new Message<int, string>
            {
                Key = partition.Value,
                Timestamp = Timestamp.Default,
                Value = message
            };

            return kafkaProducer.ProduceAsync(kafkaTopic, kafkaMessage);
        }
    }
}
