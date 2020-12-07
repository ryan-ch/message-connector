using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Threading.Tasks;

namespace XB.Kafka
{
    public class KafkaProducer : IKafkaProducer
    {
        public IConfiguration Configuration { get; set; }
        public ProducerConfig Config { get; }
        public IProducer<int, string> kafkaProducer { get; set; }
        public string Topic { get; }

        public KafkaProducer(IConfiguration configuration)
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
            kafkaProducer = new ProducerBuilder<int, string>(Config).Build();
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

            await kafkaProducer.ProduceAsync(kafkaTopic, kafkaMessage);
        }
    }
}
