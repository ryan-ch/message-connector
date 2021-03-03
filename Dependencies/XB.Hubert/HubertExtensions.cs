using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using XB.HttpClientJwt;
using XB.Hubert.Config;

namespace XB.Hubert
{
    public static class HubertExtensions
    {
        public static IServiceCollection AddHubert(this IServiceCollection services, IConfiguration configuration, string appsettingsPrefix = "")
        {
            return services.Configure<HubertClientOptions>(configuration.GetSection(appsettingsPrefix + HubertClientOptions.ConfigurationSection))
            .AddHttpClientJwt(configuration, appsettingsPrefix, HubertClientOptions.HttpClientIdentifier)
            .AddScoped<IHubertClient, HubertClient>();
        }
    }
}
