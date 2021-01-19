using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using XB.Kafka;

namespace KafkaProducerTool
{
    class Program
    {
        static async Task Main(string[] args)
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
