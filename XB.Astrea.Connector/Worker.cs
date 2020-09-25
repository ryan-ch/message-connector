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
            _mqClient.Start();

            for (int i = 0; i < 1000; i++)
            {
                await _mqClient.WriteMessageAsync("{1:F01ESSEDKK0AXXX8000024323}{2:O1030924131007ESSEDEF0AXXX29952796451401070710N}{3:{108:PTS1861098}{119:STP}}{4:  :20:TestAR2796  :23B:CRED  :32A:140107DKK2796  :33B:DKK472694,44  :50A:/DE65510104009000132915  AARBDE5WXXX  :52A:TDOMCATTTOR  :57A:HANDDKK0XXX  :59:/DK1063010002029774  THE BENEFICIARY  ADDRESS  POSTAL CODE  COUNTRY  :70:INC MT103, 00, On-pass from  Vostro to Domestic Bank, FIN Copy  :71A:SHA  -}", new CancellationToken());
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                await _mqClient.ReceiveMessageAsync(stoppingToken);
            }

            _mqClient.Stop();
        }
    }
}
