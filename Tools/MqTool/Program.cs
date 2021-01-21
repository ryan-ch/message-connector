using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using XB.IBM.MQ.Interfaces;

namespace MqTool
{
    internal class Program
    {
        private static async Task Main()
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
                            mqClientProducer.WriteMessage(@"{1:F01ESSESES0AXXX8000019102}{2:O1030955100518CIBCCATTAXXX76763960792012021544N}{3:{108:78}{111:001}{121:98408af9-695e-4828-9b63-24e63cacb8eb}}{4:
:20:XMEC-CAD
:23B:CRED
:32A:200723CAD224,28
:33B:CAD224,28
:50K:/9991
ONE OF OUR CUSTOMERS
:59:/52801010480
BENEF
:70:BETORSAK
:71A:SHA
-}{S:{MAN:UAKOUAK4600}}");
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
