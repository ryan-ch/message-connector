using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using XB.HttpClientJwt;
using XB.Hubert;
using XB.Hubert.Config;

namespace HubertTool
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
            services.AddSingleton(configuration);
            services.AddHttpClientJwt(configuration, HubertClientOptions.HttpClientIdentifier);
            services.AddHubert(configuration);


            // build the pipeline
            Provider = services.BuildServiceProvider();

        }
    }
}
