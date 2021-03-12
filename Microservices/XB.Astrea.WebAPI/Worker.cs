using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using XB.Astrea.Client;
using XB.IBM.MQ.Interfaces;

namespace XB.Astrea.WebAPI
{
    public class Worker : BackgroundService
    {
        private const int waitTimeMs = 1000;

        private readonly ILogger<Worker> _logger;
        private readonly IMqConsumer _mqConsumer;
        private readonly IServiceProvider _services;

        public Worker(ILogger<Worker> logger, IMqConsumer mqConsumer, IServiceProvider services)
        {
            _logger = logger;
            _mqConsumer = mqConsumer;
            _services = services;

            using var scope = _services.CreateScope();
            var backgroundServiceController = scope.ServiceProvider.GetRequiredService<BackgroundServicesManager>();
            backgroundServiceController.RegisterBackgroundService(this);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(() =>
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var messageReceived = false;
                    try
                    {
                        var message = _mqConsumer.ReceiveMessage(waitTimeMs);
                        if (string.IsNullOrEmpty(message))
                            continue;

                        bool.TryParse(Environment.GetEnvironmentVariable("DryRun"), out bool dryRun);
                        if (!dryRun)
                            _ = HandleMessage(message);
                        else
                            _logger.LogInformation($"Processed message {message} in dry run mode");
                        messageReceived = true;
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, e.Message);
                    }
                    finally
                    {
                        if (messageReceived)
                            _mqConsumer.Commit();
                    }
                }
                return Task.CompletedTask;
            }, stoppingToken);
        }

        private async Task HandleMessage(string message)
        {
            try
            {
                using var scope = _services.CreateScope();
                var scopedAstreaClient = scope.ServiceProvider.GetRequiredService<IAstreaClient>();
                await scopedAstreaClient.AssessAsync(message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }
        }
    }
}
