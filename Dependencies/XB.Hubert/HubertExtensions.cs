using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using System;
using XB.HttpClientJwt;
using XB.HttpClientJwt.Config;
using XB.Hubert.Config;

namespace XB.Hubert
{
    public static class HubertExtensions
    {
        public static IServiceCollection AddHubert(this IServiceCollection services, IConfiguration configuration)
        {
            var retryAttempts = configuration.GetValue(HubertClientOptions.ConfigurationSection + ":RetryAttempts", 2);
            var retrySleepDuration = configuration.GetValue<double>(HubertClientOptions.ConfigurationSection + ":RetrySleepDuration", 10);

            var retryPolicy = HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(retryAttempts, _ => TimeSpan.FromSeconds(retrySleepDuration));

            services.Configure<HubertClientOptions>(configuration.GetSection(HubertClientOptions.ConfigurationSection))
                .Configure<HttpClientJwtOptions>(configuration.GetSection(HttpClientJwtOptions.ConfigurationSection))
                .AddScoped<IHubertClient, HubertClient>()
                .AddHttpClientJwt(configuration, HubertClientOptions.HttpClientIdentifier)
                .AddPolicyHandler(retryPolicy);

            return services;
        }
    }
}
