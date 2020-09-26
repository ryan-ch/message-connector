using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using XB.IBM.MQ;

namespace MqWriter
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly MqClient _mqClient;

        public Worker(ILogger<Worker> logger, MqClient mqClient)
        {
            _logger = logger;
            _mqClient = mqClient;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                _mqClient.Start();

                Stopwatch stopwatch = new Stopwatch();

                stopwatch.Start();
                for (var i = 0; i < 2000; i++)
                {
                    await _mqClient.WriteMessageAsync("{1:F01ESSEDKK0AXXX8000024323}{2:O1030924131007ESSEDEF0AXXX29952796451401070710N}{3:{108:PTS1861098}{119:STP}}{4:  :20:TestAR2796  :23B:CRED  :32A:140107DKK2796  :33B:DKK472694,44  :50A:/DE65510104009000132915  AARBDE5WXXX  :52A:TDOMCATTTOR  :57A:HANDDKK0XXX  :59:/DK1063010002029774  THE BENEFICIARY  ADDRESS  POSTAL CODE  COUNTRY  :70:INC MT103, 00, On-pass from  Vostro to Domestic Bank, FIN Copy  :71A:SHA  -}", stoppingToken);
                }
                stopwatch.Stop();

                _mqClient.Stop();

                _logger.LogInformation("Time elapsed: {0}", stopwatch.Elapsed);
            }
        }
    }
}
