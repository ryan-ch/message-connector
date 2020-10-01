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
        private readonly IMqConsumer _mqConsumer;
        private readonly IMqProducer _mqProducer;
        private readonly IAstreaClient _astreaClient;

        public Worker(ILogger<Worker> logger, IMqConsumer mqConsumer, IAstreaClient astreaClient, IMqProducer mqProducer)
        {
            _logger = logger;
            _mqConsumer = mqConsumer;
            _astreaClient = astreaClient;
            _mqProducer = mqProducer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _mqConsumer.Start();

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var counter = 0;

            while (!stoppingToken.IsCancellationRequested && counter < 100)
            {
                var message = _mqConsumer.ReceiveMessage();

                if (message != string.Empty)
                {
                    //var assess = await _astreaClient.Assess(message);

                    //if (assess.AssessmentStatus == "OK")
                    //{
                        _mqProducer.WriteMessage(counter.ToString());
                    //}
                }

                counter++;

                if (counter % 10 == 0)
                {
                    _mqConsumer.Commit();
                    _mqProducer.Commit();
                }
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
