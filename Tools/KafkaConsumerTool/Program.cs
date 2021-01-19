using System;
using Microsoft.Extensions.DependencyInjection;
using XB.Kafka;

namespace KafkaConsumerTool
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("============ Kafka Consumer ============");

            var _startUp = new Startup();
            var kafkaConsumerClient = _startUp.Provider.GetRequiredService<IKafkaConsumer>();
            while (true)
            {
                kafkaConsumerClient.Consume("ses-fcp-payment-orders");
            }
            
        }
    }
}
