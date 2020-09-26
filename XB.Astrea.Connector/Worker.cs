using System;
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
        private readonly IAstreaClient _astreaClient;

        public Worker(ILogger<Worker> logger, IMqClient mqClient, IAstreaClient astreaClient)
        {
            _logger = logger;
            _mqClient = mqClient;
            _astreaClient = astreaClient;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _mqClient.Start();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            while (!stoppingToken.IsCancellationRequested)
            {
                var message = await _mqClient.ReceiveMessageAsync(stoppingToken);
                _logger.LogInformation(DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ": Message read");

                //if (message != string.Empty)
                //{
                //    string astreaResponse = await _astreaClient.SayHelloAsync();
                //}
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
