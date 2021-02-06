using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using IBM.XMS;
using XB.IBM.MQ.Interfaces;

namespace MqTool
{
    internal class Program
    {
        public static void Main()
        {
            var startup = new Startup();
            var mqClientProducer = startup.Provider.GetRequiredService<IMqProducer>();
            var mqClientConsumer = startup.Provider.GetRequiredService<IMqConsumer>();

            bool continueProgram = true;

            while (continueProgram)
            {
                Console.WriteLine("What do you want to do?");
                Console.WriteLine("1. Write messages?");
                Console.WriteLine("2. Consume messages?");
                Console.WriteLine("0. Quit");
                Console.Write(": ");

                string input = Console.ReadLine();

                var timer = new Stopwatch();
                timer.Start();

                switch (input)
                {
                    case "1":
                        Console.Write("How many messages?: ");
                        int writes = int.Parse((Console.ReadLine()));
                        for (int i = 0; i < writes; i++)
                        {
                            mqClientProducer.WriteMessage(@"{1:F01ESSESES0AXXX8000019102}{2:O1030955100518CIBCCATTAXXX76763960792012021544N}{3:{103:}{108:WA SQ9E3P}{119:}{111:001}{121:2e66e52d-5448-4742-875a-c39a844bbdc2}}{4:
:20:GEcG
:23B:CRED
:32A:200825SEK3500,00
:50F:/SE2880000832790000012345
1/Vårgårda Kromverk
2/Lilla Korsgatan 3
7/AZ/ynai3oTv8DtC91iwYm87b-vXtWBhRG
8/ynai3oTv8DtC91iwYm87b-vXtWBhRG
:59F:/SE3550000000054910123123
1/BOB BAKER
2/Bernhards Gränd 3, 418 42 Göteborg
4/19920914
6/BQ/1zWLCaVqFd3Rs/47281128569335
:71A:SHA
:72:/REC/RETN
-}{S:{MAN:UAKOUAK4600}}");
                            if (i % 100 == 0) mqClientProducer.Commit();
                        }
                        mqClientProducer.Commit();
                        break;
                    case "2":
                        Console.Write("How many messages?: ");
                        int reads = int.Parse((Console.ReadLine()));
                        
                        for (int i = 0; i < reads; i++)
                        {
                            mqClientConsumer.ReceiveMessage();
                        }

                        break;
                    case "0":
                        continueProgram = false;
                        break;

                }
                timer.Stop();

                var timeTaken = timer.Elapsed;
                Console.WriteLine("Time taken: " + timeTaken.ToString(@"m\:ss\.fff"));
            }
        }
    }
}
