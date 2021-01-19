using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using XB.Kafka;
using XB.Kafka.Config;

namespace KafkaProducerTool
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public IServiceProvider Provider { get; }

        public Startup()
        {

            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.Development.json", optional: true)
                .Build();

            var services = new ServiceCollection();

            // add necessary services
            services.AddLogging(configure => configure.AddConsole());
            services.AddSingleton(_configuration);
            services.AddKafkaProducer(_configuration, "AppSettings:");

            // build the pipeline
            Provider = services.BuildServiceProvider();
            
        }

        
        

    }
}
