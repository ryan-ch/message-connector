using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using XB.Astrea.Client.Config;
using XB.Hubert;
using XB.Kafka;
using XGalaxy.Common.Polly;

namespace XB.Astrea.Client
{
    public static class AstreaClientExtensions
    {
        internal const string HttpClientName = "astrea";

        public static IServiceCollection AddAstreaClient(this IServiceCollection services, IConfiguration configuration)
        {
            var retryAttempts = configuration.GetValue(AstreaClientOptions.ConfigurationSection + "RetryAttempts", 30);
            var retrySleepDuration = configuration.GetValue<double>(AstreaClientOptions.ConfigurationSection + "RetrySleepDuration", 10);

            services.AddHttpClient(HttpClientName, c =>
                {
                    c.BaseAddress = new Uri(configuration[AstreaClientOptions.ConfigurationSection + ":Url"]);
                    c.DefaultRequestHeaders.Add("Accept", "application/json");
                    c.DefaultRequestVersion = HttpVersion.Version20;
                })
                .AddPolicyHandler(PollyExtension.GetRetryPolicy<AstreaClient>(retryAttempts, retrySleepDuration));

            return services
                .Configure<AstreaClientOptions>(configuration.GetSection(AstreaClientOptions.ConfigurationSection))
                .AddScoped<IAstreaClient, AstreaClient>()
                .AddKafkaProducer(configuration)
                .AddHubert(configuration);
            //.AddKafkaConsumer(configuration)
        }
    }
}
