using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;

namespace XB.Kafka.Config
{
    public class KafkaConsumerConfig: ConsumerConfig
    {
        public const string ConfigurationSection = "KafkaConsumer";

        public KafkaConsumerConfig()
        {
            GroupId = Guid.NewGuid().ToString(); //Leave as a guid for now 
            SecurityProtocol = Confluent.Kafka.SecurityProtocol.Ssl;
            SaslMechanism = Confluent.Kafka.SaslMechanism.ScramSha512;
            AutoCommitIntervalMs = 2000;
            EnableAutoCommit = true;
        }
    }
}
