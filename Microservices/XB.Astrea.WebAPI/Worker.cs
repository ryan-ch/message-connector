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
        private const int waitTimeMs = 0;

        private readonly ILogger<Worker> _logger;
        private readonly IMqConsumer _mqConsumer;
        private readonly IServiceProvider _services;
        private readonly bool _debugging;

        public Worker(ILogger<Worker> logger, IMqConsumer mqConsumer, IServiceProvider services)
        {
            _logger = logger;
            _mqConsumer = mqConsumer;
            _services = services;

            using var scope = _services.CreateScope();
            var backgroundServiceController = scope.ServiceProvider.GetRequiredService<BackgroundServicesManager>();
            backgroundServiceController.RegisterBackgroundService(this);
            _ = bool.TryParse(Environment.GetEnvironmentVariable("Debugging"), out _debugging);
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
                        if (_debugging)
                            _logger.LogInformation("Running worker in debug mode");
                        var message = _mqConsumer.ReceiveMessage(waitTimeMs);
                        if (string.IsNullOrEmpty(message))
                        {
                            _logger.LogInformation($"No message received: {message}");
                            continue;
                        }

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
                        {
                            if (_debugging)
                                _logger.LogInformation("Exiting task after committing message");
                            _mqConsumer.Commit();
                        }
                        else
                        {
                            if (_debugging)
                                _logger.LogInformation("Exited task without message received");
                        }
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
