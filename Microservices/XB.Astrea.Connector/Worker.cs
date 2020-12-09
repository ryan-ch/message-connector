using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using XB.Astrea.Client;
using XB.IBM.MQ.Interfaces;

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
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    string message = _mqConsumer.ReceiveMessage();

                    if (!string.IsNullOrEmpty(message))
                    {
                        _logger.LogInformation(message);
                        var assess = await _astreaClient.AssessAsync(message);
                        _mqConsumer.Commit();
                    }
                }
                catch (Exception)
                {
                    _mqConsumer.Rollback();
                }
            }
        }
    }
}
