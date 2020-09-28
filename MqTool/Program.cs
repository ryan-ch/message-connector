using System;
using System.IO;
using System.Reflection;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using XB.IBM.MQ;

namespace MqTool
{
    class Program
    {
        static void Main(string[] args)
        {
            var startup = new Startup();
            var mqClient = startup.Provider.GetService<IMqClient>();
            mqClient.Start();

            bool continueProgram = true;

            while (continueProgram)
            {
                Console.WriteLine("What do you want to do?");
                Console.WriteLine("1. Write messages?");
                Console.WriteLine("0. Quit");
                Console.Write(": ");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        Console.Write("How many messages?: ");
                        int iterations = Int32.Parse((Console.ReadLine()));
                        for (int i = 0; i < iterations; i++)
                        {
                            mqClient.WriteMessageAsync("test " + i, new CancellationToken());
                        }
                        break;
                    case "0":
                        continueProgram = false;
                        break;

                }
            }

            mqClient.Stop();
        }
    }
}
