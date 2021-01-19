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

        public static IServiceCollection AddAstreaClient(this IServiceCollection services, IConfiguration configuration, string appsettingsPrefix = "")
        {
            services.AddHttpClient(HttpClientName, c =>
            {
                c.BaseAddress = new Uri(configuration["AppSettings:Astrea:Url"]);
                c.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            return services
                .AddScoped<IAstreaClient, AstreaClient>()
                .AddKafkaProducer(configuration, appsettingsPrefix);
                //.AddKafkaConsumer(configuration, appsettingsPrefix)
        }

    }
}
