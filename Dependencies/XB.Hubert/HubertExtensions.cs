using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Polly.Retry;
using System;
using System.Net.Http;
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

            services.Configure<HubertClientOptions>(configuration.GetSection(HubertClientOptions.ConfigurationSection))
                .Configure<HttpClientJwtOptions>(configuration.GetSection(HttpClientJwtOptions.ConfigurationSection))
                .AddScoped<IHubertClient, HubertClient>()
                .AddHttpClientJwt(configuration, HubertClientOptions.HttpClientIdentifier)
                .AddPolicyHandler(GetRetryPolicy(retryAttempts, retrySleepDuration));

            return services;
        }
        
        internal static AsyncRetryPolicy<HttpResponseMessage> GetRetryPolicy(int retryAttempts, double retrySleepDuration)
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(retryAttempts, _ => TimeSpan.FromSeconds(retrySleepDuration));
        }
    }
}
