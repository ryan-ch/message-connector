using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PEX.Connectors.MQ.Reader;
using PEX.Connectors.MQAdapter;
using PMC.Common.Logging;
using PMC.Common.Threading;
using PMC.Common.Wrappers;
using XB.Astrea.Connector.Logger;

namespace XB.Astrea.Connector
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddTransient<IMqAdapterFactory, MqAdapterFactory>();
                    services.AddTransient<IMqQueuePoller, MqQueuePoller>();
                    services.AddTransient<IClientLogger, ClientLogger>();
                    services.AddTransient<IThreadExceptionHandler, ThreadExceptionHandler>();
                    services.AddTransient<IThreadSleep, ThreadSleepWrapper>();
                    services.AddTransient<IActionExceptionHandler, ActionExceptionHandler>();
                    services.AddTransient<ITaskFactory, TaskFactory>();
                    services.AddTransient<IMqMessageBuilder, MqMessageBuilder>();

                    services.AddTransient<IMqQueueProcessor, MqQueueProcessor>();
                    services.AddTransient<IConfigurationManagerWrapper, ConfigurationManagerWrapper>();

                    services.AddSingleton<IMqQueueManagerFactory, MqQueueManagerFactory>();

                    services.AddTransient<IMqReader, LocalMqReader>();

                    services.AddHostedService<Worker>();
                });
    }
}
