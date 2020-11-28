using Confluent.Kafka;
using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace XB.Kafka
{
    public class Producer : IProducer
    {
        public IConfiguration Configuration { get; set; }
        public ProducerConfig Config { get; }
        public IProducer<int, string> KafkaProducer { get; set; }
        public string Topic { get; }

        public Producer(IConfiguration configuration)
        {
            Configuration = configuration;
            Config = new ProducerConfig
            {
                Acks = Acks.None,
                ApiVersionRequest = true,
                BootstrapServers = Configuration["AppSettings:Kafka:BootstrapServers"],
                SecurityProtocol = SecurityProtocol.Ssl,
                SaslMechanism = SaslMechanism.ScramSha512,
                SslCaLocation = Configuration["AppSettings:Kafka:SslCaLocation"],
                SslCertificateLocation = Configuration["AppSettings:Kafka:SslCertificateLocation"],
                SslKeyLocation = Configuration["AppSettings:Kafka:SslKeyLocation"],
                SslKeystoreLocation = Configuration["AppSettings:Kafka:SslKeystoreLocation"],
                SslKeystorePassword = Configuration["AppSettings:Kafka:SslKeystorePassword"],
                SslKeyPassword = Configuration["AppSettings:Kafka:SslKeyPassword"],
                ClientId = Dns.GetHostName()
            };
            Topic = Configuration["AppSettings:Kafka:Topic"];
            KafkaProducer = new ProducerBuilder<int, string>(Config).Build();
        }

        public async Task Execute(string message)
        {
            var partition = new Partition(0);
            var kafkaTopic = new TopicPartition(Topic, partition);

            var kafkaMessage = new Message<int, string>
            {
                Key = partition.Value,
                Timestamp = Timestamp.Default,
                Value = message
            };

            await KafkaProducer.ProduceAsync(kafkaTopic, kafkaMessage);
        }
    }
}
