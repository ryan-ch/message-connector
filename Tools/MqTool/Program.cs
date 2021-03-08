using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using XB.IBM.MQ.Interfaces;
using XB.MtParser.Interfaces;

namespace MqTool
{
    internal class Program
    {
        public static void Main()
        {
            string mtMessage1 = @"{1:F01ESSESES0AXXX8000019102}{2:O1030955100518CIBCCATTAXXX7676396079" + DateTime.Now.ToString("yyMMddHHmm") + @"N}{3:{103:}{108:WA SQ9E3P}{119:}{111:001}{121:"+ Guid.NewGuid().ToString() +@"}}{4:
:20:GEcG
:23B:CRED
:32A:200825SEK3500,00
:50F:/SE2880000832790000012345
1/Vårgårda Kromverk
2/Lilla Korsgatan 3
4/19920914
6/BQ/1zWLCaVqFd3Rs/47281128569335
7/AZ/ynai3oTv8DtC91iwYm87b-vXtWBhRG
8/ynai3oTv8DtC91iwYm87b-vXtWBhRG
:59F:/SE3550000000054910123123
1/BOB BAKER
2/Bernhards Gränd 3, 418 42 Göteborg
2/TRANSVERSAL 93 53 48 INT 70
3/CO/BOGOTA
:71A:SHA
:72:/REC/RETN
-}{S:{MAN:UAKOUAK4600}}";
            string mtMessage2 = @"{1:F01ESSESES0AXXX8000025977}{2:O1030955100518IRVTUS3NAXXX7676396079" + DateTime.Now.ToString("yyMMddHHmm") + @"N}{3:{108:78}{121:" + Guid.NewGuid().ToString() + @"}}{4:
:20:RS202102158
:23B:CRED
:32A:210215SEK12,00
:33B:SEK12,00
:50K:/DE89370400440532013000
EUB
:59:/50601001079
BENEF
:70:BETORSAK
:71A:SHA
-}{S:{MAN:UAKOUAK4600}}";

            var startup = new Startup();
            var mqClientProducer = startup.Provider.GetRequiredService<IMqProducer>();
            var mqClientConsumer = startup.Provider.GetRequiredService<IMqConsumer>();
            var newMtParser = startup.Provider.GetRequiredService<IMTParser>();

            var continueProgram = true;

            while (continueProgram)
            {
                Console.WriteLine("What do you want to do?");
                Console.WriteLine("1. Write messages?");
                Console.WriteLine("2. Consume messages?");
                Console.WriteLine("3. Test parser performance");
                Console.WriteLine("0. Quit");
                Console.Write(": ");

                var input = Console.ReadLine();
                var timer = new Stopwatch();

                switch (input)
                {
                    case "1":
                        Console.Write("How many messages?: ");
                        var writes = int.Parse(Console.ReadLine()!);
                        timer.Start();
                        for (var i = 0; i < writes; i++)
                        {
                            mqClientProducer.WriteMessage(i % 2 == 0 ? mtMessage1 : mtMessage2);
                            if (i % 100 == 0) mqClientProducer.Commit();
                        }
                        mqClientProducer.Commit();
                        timer.Stop();
                        break;

                    case "2":
                        Console.Write("How many messages?: ");
                        var reads = int.Parse(Console.ReadLine()!);
                        timer.Start();
                        for (var i = 0; i < reads; i++)
                        {
                            mqClientConsumer.ReceiveMessage();
                        }
                        timer.Stop();
                        break;

                    case "3":
                        Console.Write("How many messages?: ");
                        var loops = int.Parse(Console.ReadLine()!);
                        timer.Start();
                        for (var i = 0; i < loops; i++)
                        {
                            _ = newMtParser.ParseSwiftMt103Message(i % 2 == 0 ? mtMessage1 : mtMessage2);
                        }
                        timer.Stop();
                        break;
                    case "4":
                        Console.Write("Enter filename/path: ");
                        var path = Console.ReadLine();
                        timer.Start();
                        var fileContents = File.ReadAllLines(path);
                        var sb = new StringBuilder();
                        var messages = new List<string>();

                        foreach (var line in fileContents)
                        {
                            if (string.IsNullOrEmpty(line))
                            {
                                messages.Add(sb.ToString());
                                sb.Clear();
                                continue;
                            }
                            sb.Append(line);
                        }
                        Console.Write($"There are {messages.Count} messages in {path}. How many messages do you want to send?: ");
                        var count = 0;
                        foreach (var message in messages.Take(int.Parse(Console.ReadLine())))
                        {
                            mqClientProducer.WriteMessage(message);
                            if (count++ % 100 == 0) mqClientProducer.Commit();
                        }
                        mqClientProducer.Commit();
                        timer.Stop();
                        break;
                    case "0":
                        continueProgram = false;
                        break;
                }

                var timeTaken = timer.Elapsed;
                Console.WriteLine("Time taken: " + timeTaken.ToString(@"m\:ss\.fff"));
            }
        }
    }
}
