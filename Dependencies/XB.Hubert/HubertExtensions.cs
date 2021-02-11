using System;
using Microsoft.Extensions.DependencyInjection;
using XB.Hubert.Config;
using Microsoft.Extensions.Configuration;


namespace XB.Hubert
{
    public static class HubertExtensions
    {
        internal const string HttpClientName = "hubert";

        public static IServiceCollection AddHubert(this IServiceCollection services, IConfiguration configuration, string appsettingsPrefix = "")
        {
            services.Configure<HubertClientOptions>(configuration.GetSection(appsettingsPrefix + HubertClientOptions.ConfigurationSection));

            services.AddHttpClient(HttpClientName, c =>
            {
                c.DefaultRequestHeaders.Add("Accept", "application/json");
                c.BaseAddress = new Uri(configuration[appsettingsPrefix + HubertClientOptions.ConfigurationSection + ":Url"]);
            });

            return services.AddScoped<IHubertClient, HubertClient>();
        }
    }
}
