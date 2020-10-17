using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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

                        if (assess.AssessmentStatus == "OK")
                        {
                            //process message and place it back to MQ
                            MqProducer.WriteMessage(message + " " + assess.AssessmentStatus);
                        }
                    }

                    MqConsumer.Commit();
                    MqProducer.Commit();
                }
                catch (Exception ex)
                {
                    MqConsumer.Rollback();
                    MqProducer.Rollback();
                    MqProducer.WriteMessage(message + " " + ex.Message);
                    MqProducer.Commit();
                }
            }
        }
    }
}
