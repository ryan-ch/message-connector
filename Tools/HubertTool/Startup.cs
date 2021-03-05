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
            services.AddSingleton(_configuration);
            services.AddHttpClientJwt(_configuration, "AppSettings:", HubertClientOptions.HttpClientIdentifier);
            services.AddHubert(_configuration, "AppSettings:");
            

            // build the pipeline
            Provider = services.BuildServiceProvider();

        }
    }
}
