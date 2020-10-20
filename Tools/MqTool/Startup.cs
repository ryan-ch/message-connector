using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using XB.IBM.MQ;

namespace MqTool
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.Development.json", optional: true)
                .Build();

            // instantiate
            var services = new ServiceCollection();

            // add necessary services
            services.AddLogging(configure => configure.AddConsole());
            services.AddSingleton(_configuration);
            services.AddMqProducer();

            // build the pipeline
            Provider = services.BuildServiceProvider();
        }

        // access the built service pipeline
        public IServiceProvider Provider { get; }
    }
}
