using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using XB.Hubert;

namespace HubertTool
{
    class Program
    {
        private static async Task Main(string[] args)
        {
            var startup = new Startup();
            var hubertClient = startup.Provider.GetRequiredService<IHubertClient>();

            Random rnd = new Random();
            
            while (true)
            {
                Console.WriteLine("Enter amount of assessments to send to Hubert: ");
                var amount = int.Parse(Console.ReadLine());

                var sTime = DateTime.Now;
                Console.WriteLine(sTime.ToString("yyyy-MM-ddTHH:mm:ss.fff"));
                
                for (int i = 0; i < amount; i++)
                {
                    string guid = Guid.NewGuid().ToString();
                    _ = Task.Run(async () =>
                    {
                        var result = await hubertClient.SendAssessmentResultAsync(DateTime.Now.ToString(),
                            guid, rnd.Next(0, 10).ToString());
                        if (result.Result != null)
                        {
                            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff") + " ===== Status from Hubert: " + result.Result.Uakw4630.TransactionStatus + " Guid: " + guid);
                        }
                    });
                }
                Console.ReadLine();
            }
        }
    }
}
