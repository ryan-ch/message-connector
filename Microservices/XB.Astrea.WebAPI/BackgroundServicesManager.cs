using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace XB.Astrea.WebAPI
{
    public class BackgroundServicesManager
    {
        private readonly List<BackgroundService> RegisteredBackgroundServices = new List<BackgroundService>();
        private readonly ILogger<BackgroundServicesManager> _logger;

        public BackgroundServicesManager(ILogger<BackgroundServicesManager> logger)
        {
            _logger = logger;
        }

        public void RegisterBackgroundService(BackgroundService bs)
        {
            if (!RegisteredBackgroundServices.Contains(bs))
                RegisteredBackgroundServices.Add(bs);

            _logger.LogInformation("BackgroundService Registered: {bs}", bs.GetType());
        }

        public async Task StartAll()
        {
            foreach (var bs in RegisteredBackgroundServices)
            {
                try
                {
                    await bs.StartAsync(new CancellationToken());
                    _logger.LogInformation("BackgroundService started: {bs}", bs.GetType());
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error when trying to start Background Service: {bs}", bs.GetType());
                }
            }
        }

        public async Task StartByName(string backgroundServiceClassName)
        {
            var bs = RegisteredBackgroundServices.FirstOrDefault(a => a.GetType().Name.ToLower() == backgroundServiceClassName.ToLower().Trim());
            try
            {
                if (bs == null)
                    throw new KeyNotFoundException(backgroundServiceClassName + " background service was not registered");

                await bs.StartAsync(new CancellationToken());
                _logger.LogInformation("BackgroundService started: {bs}", bs.GetType());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when trying to start Background Service: {backgroundServiceClassName}", backgroundServiceClassName);
            }
        }

        public async Task StopAll()
        {
            foreach (var bs in RegisteredBackgroundServices)
            {
                try
                {
                    await bs.StopAsync(new CancellationToken());
                    _logger.LogInformation("BackgroundService stopped: {bs}", bs.GetType());
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error when trying to stop Background Service: {bs}", bs.GetType());
                }
            }
        }

        public async Task StopByName(string backgroundServiceClassName)
        {
            var bs = RegisteredBackgroundServices.FirstOrDefault(a => a.GetType().Name.ToLower() == backgroundServiceClassName.ToLower().Trim());
            try
            {
                if (bs == null)
                    throw new KeyNotFoundException(backgroundServiceClassName + " background service was not registered");

                await bs.StopAsync(new CancellationToken());
                _logger.LogInformation("BackgroundService stoped: {bs}", bs.GetType());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when trying to stop Background Service: {backgroundServiceClassName}", backgroundServiceClassName);
            }
        }
    }
}