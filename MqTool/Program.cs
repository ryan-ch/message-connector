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
            var mqClient = startup.Provider.GetRequiredService<IMqClientWriter>();

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
                            mqClient.WriteMessage(@"{1:F01ESSESES0AXXX8000427729}{2:O1030955100518ANZBAU3MAXXX76763960792009301009N}{3:{108:78}{111:001}{121:b5c8ae42-5e1b-44da-909d-c145baab4394}}{4:
:20:MG-AUD
:23B:CRED
:32A:200930AUD71,01
:33B:AUD71,01
:50K:/9991
ONE OF OUR CUSTOMERS
NAME AND ADDRESS
:52A:IRVTUS3N
:53A:ANZBAU30
:59:/54401010043
BENEFICIARY NAME
BENEFICIARY ADRESS
:70:BETORSAK RAD 1
BETORSAK RAD 2
:71A:SHA
-}{S:{MAN:UAKOUAK4600}}");
                        }
                        break;
                    case "0":
                        continueProgram = false;
                        break;

                }
            }
        }
    }
}
