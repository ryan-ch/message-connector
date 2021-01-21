using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using XB.IBM.MQ.Interfaces;

namespace XB.Astrea.WebAPI
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IMqConsumer _mqConsumer;
        private readonly IServiceProvider _services;

        public Worker(ILogger<Worker> logger, IMqConsumer mqConsumer, IServiceProvider services)
        {
            _logger = logger;
            _mqConsumer = mqConsumer;
            _services = services;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    string message = _mqConsumer.ReceiveMessage();
                    if (!string.IsNullOrEmpty(message))
                        _ = HandleMessage(message);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, e.Message);
                }
                finally
                {
                    _mqConsumer.Commit();
                }
            }
            return Task.CompletedTask;
        }

        private async Task HandleMessage(string message)
        {
            _logger.LogInformation(message);

            //temporarily disable astrea client until its ready
            await Task.Delay(2000).ConfigureAwait(false);
            _logger.LogWarning("Done");

            //try
            //{
            //    using var scope = _services.CreateScope();
            //    var scopedAstreaClient = scope.ServiceProvider.GetRequiredService<IAstreaClient>();
            //    await scopedAstreaClient.AssessAsync(message).ConfigureAwait(false);
            //}
            //catch (Exception e)
            //{
            //    _logger.LogError(e, e.Message);
            //}
        }
    }
}
