using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using XB.Kafka;

namespace KafkaConsumerTool
{
    public class Startup
    {
        public IServiceProvider Provider { get; }

        public Startup()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json", optional: true)
                .Build();

            var services = new ServiceCollection();

            // add necessary services
            services.AddLogging(configure => configure.AddConsole());
            services.AddSingleton(configuration);
            services.AddKafkaConsumer(configuration);

            // build the pipeline
            Provider = services.BuildServiceProvider();


        }
    }
}
