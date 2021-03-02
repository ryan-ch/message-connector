using Microsoft.Extensions.DependencyInjection;
using XB.Hubert.Config;
using Microsoft.Extensions.Configuration;
using XB.HttpClientJwt;

namespace XB.Hubert
{
    public static class HubertExtensions
    {
        public static IServiceCollection AddHubert(this IServiceCollection services, IConfiguration configuration, string appsettingsPrefix = "")
        {
            services.Configure<HubertClientOptions>(configuration.GetSection(appsettingsPrefix + HubertClientOptions.ConfigurationSection));
            services.AddHttpClientJwt(configuration, appsettingsPrefix, HubertClientOptions.HttpClientIdentifier);
            return services.AddTransient<IHubertClient, HubertClient>();
        }
    }
}
