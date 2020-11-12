using System;
using System.Diagnostics;
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
        public ILogger<Worker> Logger { get; }
        public IMqConsumer MqConsumer { get; }
        public IMqProducer MqProducer { get; }
        public IAstreaClient AstreaClient { get; }

        public Worker(ILogger<Worker> logger, IMqConsumer mqConsumer, IAstreaClient astreaClient, IMqProducer mqProducer)
        {
            Logger = logger;
            MqConsumer = mqConsumer;
            AstreaClient = astreaClient;
            MqProducer = mqProducer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var message = string.Empty;
                try
                {
                    message = MqConsumer.ReceiveMessage();

                    if (message != string.Empty)
                    {
                        var assess = await AstreaClient.AssessAsync(message);

                        //var assess = new { AssessmentStatus = "OK" };

                        MqConsumer.Commit();
                    }
                }
                catch (Exception)
                {
                    MqConsumer.Rollback();
                }
            }
        }
    }
}
