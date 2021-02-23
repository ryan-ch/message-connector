using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using XB.Astrea.Client.Config;
using XB.Kafka;

namespace XB.Astrea.Client
{
    public static class AstreaClientExtensions
    {
        internal const string HttpClientName = "astrea";

        public static IServiceCollection AddAstreaClient(this IServiceCollection services, IConfiguration configuration, string appSettingsPrefix = "")
        {
            services.Configure<AstreaClientOptions>(configuration.GetSection(appSettingsPrefix + AstreaClientOptions.ConfigurationSection));

            services.AddHttpClient(HttpClientName, c =>
            {
                c.BaseAddress = new Uri(configuration[appSettingsPrefix + AstreaClientOptions.ConfigurationSection + ":Url"]);
                c.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            return services
                .AddScoped<IAstreaClient, AstreaClient>()
                .AddKafkaProducer(configuration, appSettingsPrefix);
            //.AddKafkaConsumer(configuration, appSettingsPrefix)
        }
    }
}
