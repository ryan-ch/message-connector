using System;
using Microsoft.Extensions.DependencyInjection;
using XB.Hubert.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using XB.Hubert.Delegate;

namespace XB.Hubert
{
    public static class HubertExtensions
    {
        internal const string HttpClientName = "hubert";

        public static IServiceCollection AddHubert(this IServiceCollection services, IConfiguration configuration, string appsettingsPrefix = "")
        {
            services.Configure<HubertClientOptions>(configuration.GetSection(appsettingsPrefix + HubertClientOptions.ConfigurationSection));
            var options = services.BuildServiceProvider().GetRequiredService<IOptions<HubertClientOptions>>();

            services.AddSingleton<AuthenticationDelegatingHandler>();
            services.AddHttpClient(HttpClientName, c =>
            {
                c.DefaultRequestHeaders.Add("Accept", "application/json");
                c.BaseAddress = new Uri(options.Value.Url);
            }).AddHttpMessageHandler<AuthenticationDelegatingHandler>();

            return services.AddScoped<IHubertClient, HubertClient>();
        }
    }
}
