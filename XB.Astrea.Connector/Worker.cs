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
            _mqClient.Start();

            //for (int i = 0; i < 1000; i++)
            //{
            //    _mqClient.WriteMessageAsync("test " + i);
            //}
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        { // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start(); 

            while (!stoppingToken.IsCancellationRequested)
            {
                await _mqClient.ReceiveMessageAsync(stoppingToken);
            }

            _mqClient.Stop();
            // Stop timing.
            stopwatch.Stop();

            // Write result.
            _logger.LogInformation("Time elapsed: {0}", stopwatch.Elapsed);
        }
    }
}
