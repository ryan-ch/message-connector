using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PEX.Connectors.MQ.Reader;
using PMC.Common.Logging;

namespace XB.Astrea.Connector
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IMqReader _mqReader;


        public Worker(ILogger<Worker> logger, IMqReader mqReader)
        {
            _logger = logger;
            _mqReader = mqReader;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await _mqReader.Start("DEV.XB.IN");
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
