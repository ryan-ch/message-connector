using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using XB.HttpClientJwt;
using XB.HttpClientJwt.Config;
using XB.Hubert.Config;
using XGalaxy.Common.Polly;

namespace XB.Hubert
{
    public static class HubertExtensions
    {
        public static IServiceCollection AddHubert(this IServiceCollection services, IConfiguration configuration)
        {
            var retryAttempts = configuration.GetValue(HubertClientOptions.ConfigurationSection + ":RetryAttempts", 3);
            var retrySleepDuration = configuration.GetValue<double>(HubertClientOptions.ConfigurationSection + ":RetrySleepDuration", 10);

            services.Configure<HubertClientOptions>(configuration.GetSection(HubertClientOptions.ConfigurationSection))
                .Configure<HttpClientJwtOptions>(configuration.GetSection(HttpClientJwtOptions.ConfigurationSection))
                .AddScoped<IHubertClient, HubertClient>()
                .AddHttpClientJwt(configuration, HubertClientOptions.HttpClientIdentifier)
                .AddPolicyHandler(PollyExtension.GetRetryPolicy<HubertClient>(retryAttempts, retrySleepDuration));

            return services;
        }
    }
}
