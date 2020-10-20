using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using XB.IBM.MQ;
using XB.IBM.MQ.Interfaces;

namespace MqTool
{
    class Program
    {
        static async Task Main()
        {
            var startup = new Startup();
            var mqClient = startup.Provider.GetRequiredService<IMqProducer>();

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
                            await mqClient.WriteMessage(@"{1:F01NKCCXH2NBXXX9712267472}{2:I103CD7BNS1A22WCN}{3:{103:YYG}{108:5V0OP4RFA66}{119:}{111:}{121:7de11583-e6e8-48b2-b8cd-771a839b7fda}}{4:
:20:cd7z1Lja3
:23B:CRED
:32A:200825SEK17500,00
:50K:/SE2880000832790000012345
Vårgårda Kromverk
Lilla Korsgatan 3
:59:/SE3550000000054910000003
Volvo Personvagnar Ab
Bernhards Gränd 3, 418 42 Göteborg
:71A:SHA
-}{5:}");
                        }
                        mqClient.Commit();
                        break;
                    case "0":
                        continueProgram = false;
                        break;

                }
            }
        }
    }
}
