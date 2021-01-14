using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using XB.Kafka;
using XB.Kafka.Config;

namespace XB.Astrea.Client
{
    public static class AstreaClientExtensions
    {
        internal const string HttpClientName = "astrea";

        public static IServiceCollection AddAstreaClientAndKafka(this IServiceCollection services, IConfiguration configuration, string appsettingsPrefix = "")
        {
            // Todo: do we need the nuget files inside this project?

            services.Configure<KafkaConfig>(configuration.GetSection(appsettingsPrefix + KafkaConfig.ConfigurationSection));

            services.AddHttpClient(HttpClientName, c =>
            {
                c.BaseAddress = new Uri(configuration["AppSettings:Astrea:Url"]);
                c.DefaultRequestHeaders.Add("Accept", "text/plain");
            });

            return services
                 .AddScoped<IKafkaProducer, KafkaProducer>()
                 .AddScoped<IAstreaClient, AstreaClient>();
        }
    }
}
