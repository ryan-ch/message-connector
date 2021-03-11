using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using XB.HttpClientJwt;
using XB.Hubert.Config;

namespace XB.Hubert
{
    public static class HubertExtensions
    {
        public static IServiceCollection AddHubert(this IServiceCollection services, IConfiguration configuration)
        {
            return services.Configure<HubertClientOptions>(configuration.GetSection(HubertClientOptions.ConfigurationSection))
            .AddHttpClientJwt(configuration, HubertClientOptions.HttpClientIdentifier)
            .AddScoped<IHubertClient, HubertClient>();
        }
    }
}
