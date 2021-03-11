using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using XB.Astrea.Client.Config;
using XB.Hubert;
using XB.Kafka;

namespace XB.Astrea.Client
{
    public static class AstreaClientExtensions
    {
        internal const string HttpClientName = "astrea";

        public static IServiceCollection AddAstreaClient(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AstreaClientOptions>(configuration.GetSection(AstreaClientOptions.ConfigurationSection));

            services.AddHttpClient(HttpClientName, c =>
            {
                c.BaseAddress = new Uri(configuration[AstreaClientOptions.ConfigurationSection + ":Url"]);
                c.DefaultRequestHeaders.Add("Accept", "application/json");
                c.DefaultRequestVersion = HttpVersion.Version20;
            });

            return services
                .AddScoped<IAstreaClient, AstreaClient>()
                .AddKafkaProducer(configuration)
                .AddHubert(configuration);
            //.AddKafkaConsumer(configuration)
        }
    }
}
