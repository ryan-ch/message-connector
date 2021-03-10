using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using XB.IBM.MQ;
using XB.MtParser;

namespace MqTool
{
    public class Startup
    {
        public Startup()
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
                .Build();

            // instantiate
            var services = new ServiceCollection();

            // add necessary services
            services.AddSingleton(configuration);
            services.AddLogging(configure => configure.AddConsole());
            services.AddMq(configuration, "AppSettings:");
            services.AddMtParser();

            // build the pipeline
            Provider = services.BuildServiceProvider();
        }

        // access the built service pipeline
        public IServiceProvider Provider { get; }
    }
}
