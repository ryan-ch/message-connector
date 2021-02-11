using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using XB.Astrea.Client.Config;
using XB.Kafka;
using XB.Hubert;

namespace XB.Astrea.Client
{
    public static class AstreaClientExtensions
    {
        internal const string HttpClientName = "astrea";

        public static IServiceCollection AddAstreaClient(this IServiceCollection services, IConfiguration configuration, string appsettingsPrefix = "")
        {
            services.Configure<AstreaClientOptions>(configuration.GetSection(appsettingsPrefix + AstreaClientOptions.ConfigurationSection));

            services.AddHttpClient(HttpClientName, c =>
            {
                c.BaseAddress = new Uri(configuration[appsettingsPrefix + AstreaClientOptions.ConfigurationSection + ":Url"]);
                c.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            return services
                .AddScoped<IAstreaClient, AstreaClient>()
                .AddKafkaProducer(configuration, appsettingsPrefix)
                .AddHubert(configuration, appsettingsPrefix);
            //.AddKafkaConsumer(configuration, appsettingsPrefix)
        }
    }
}
