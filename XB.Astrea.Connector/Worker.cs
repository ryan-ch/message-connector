using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using XB.IBM.MQ;

namespace XB.Astrea.Connector
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IMqClient _mqClient;

        public Worker(ILogger<Worker> logger, IMqClient mqClient)
        {
            _logger = logger;
            _mqClient = mqClient;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            int iterations = 2000;

            _mqClient.Start();

            for (var i = 0; i < iterations; i++)
            {
                await _mqClient.WriteMessageAsync("{1:F01ESSEDKK0AXXX8000024323}{2:O1030924131007ESSEDEF0AXXX29952796451401070710N}{3:{108:PTS1861098}{119:STP}}{4:  :20:TestAR2796  :23B:CRED  :32A:140107DKK2796  :33B:DKK472694,44  :50A:/DE65510104009000132915  AARBDE5WXXX  :52A:TDOMCATTTOR  :57A:HANDDKK0XXX  :59:/DK1063010002029774  THE BENEFICIARY  ADDRESS  POSTAL CODE  COUNTRY  :70:INC MT103, 00, On-pass from  Vostro to Domestic Bank, FIN Copy  :71A:SHA  -}", new CancellationToken());
            }

            int counter = 0;

            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            while (!stoppingToken.IsCancellationRequested && counter < iterations)
            {
                var message = await _mqClient.ReceiveMessageAsync(stoppingToken);

                if (message != string.Empty)
                {
                    counter++;
                }
            }

            stopwatch.Stop();


            _mqClient.Stop();

            _logger.LogInformation("Time elapsed: {0}", stopwatch.Elapsed);
        }

        private async Task Log(string message)
        {
            await Task.Run(() =>
            {
                _logger.LogInformation(message);
            });
        }
    }
}
