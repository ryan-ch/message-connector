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
        private readonly IMqClientReader _mqClient;
        private readonly IAstreaClient _astreaClient;

        public Worker(ILogger<Worker> logger, IMqClientReader mqClient, IAstreaClient astreaClient)
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

            var counter = 0;

            while (!stoppingToken.IsCancellationRequested && counter < 1000)
            {
                var message = _mqClient.ReceiveMessage();

                //if (message != string.Empty)
                //{
                //    string astreaResponse = await _astreaClient.SayHelloAsync();
                //}
                counter++;
            }

            stopwatch.Stop();

            _logger.LogInformation($"Time elapsed: {stopwatch.Elapsed / counter} {stopwatch.Elapsed}");
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
