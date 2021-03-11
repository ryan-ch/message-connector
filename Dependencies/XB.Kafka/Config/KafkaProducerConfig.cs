using Confluent.Kafka;
using System;

namespace XB.Kafka.Config
{
    public class KafkaProducerConfig : ProducerConfig
    {
        public const string ConfigurationSection = "KafkaProducer";

        public KafkaProducerConfig()
        {
            Acks = Confluent.Kafka.Acks.All;
            ApiVersionRequest = true;
            SecurityProtocol = Confluent.Kafka.SecurityProtocol.Ssl;
            SaslMechanism = Confluent.Kafka.SaslMechanism.ScramSha512;
            ClientId = Environment.MachineName;
        }

        public string Topic { get; set; }
    }
}
