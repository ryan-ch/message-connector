using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using XB.Kafka.Config;

namespace XB.Kafka
{
    public class KafkaConsumer:IKafkaConsumer
    {
        public IConsumer<string,string> kafkaConsumer { get; set; }

        public KafkaConsumer(IOptions<KafkaConsumerConfig> configuration)
        {
            kafkaConsumer = new ConsumerBuilder<string, string>(configuration.Value).Build();
        }
        public void Consume(string topic)
        {
            kafkaConsumer.Subscribe(topic);
            //TODO: Should there be a loop in this part of the library?
            try
            {
                var cr = kafkaConsumer.Consume();
                Console.WriteLine($"Consumed message '{cr.Message.Value}' at: '{cr.TopicPartitionOffset}'.");
            }
            catch (ConsumeException e)
            {
                Console.WriteLine($"Error occured: {e.Error.Reason}");
            }
            catch (OperationCanceledException) 
            {
                // Ensure the consumer leaves the group cleanly and final offsets are committed.
                kafkaConsumer.Close();
            }
        }
    }
}
