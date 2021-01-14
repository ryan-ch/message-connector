using Confluent.Kafka;
using System.Net;

namespace XB.Kafka.Config
{
    public class KafkaConfig : ProducerConfig
    {
        public const string ConfigurationSection = "Kafka";

        public KafkaConfig()
        {
            Acks = Confluent.Kafka.Acks.None;
            ApiVersionRequest = true;
            SecurityProtocol = Confluent.Kafka.SecurityProtocol.Ssl;
            SaslMechanism = Confluent.Kafka.SaslMechanism.ScramSha512;
            ClientId = Dns.GetHostName();//Todo: how does this work?
        }

        public string Topic { get; set; }
    }
}
