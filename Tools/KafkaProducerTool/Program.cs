using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using XB.Kafka;

namespace KafkaProducerTool
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var _startUp = new Startup();
            var kafkaProducerClient = _startUp.Provider.GetRequiredService<IKafkaProducer>();
            while (true)
            {
                Console.Write("Enter message: ");
                string input = Console.ReadLine();
                await kafkaProducerClient.Execute(input);
            }
            
        }

        
    }
}
