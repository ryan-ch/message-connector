using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using XB.HttpClientJwt.Config;

namespace XB.HttpClientJwt
{
    public static class HttpClientJwtExtensions
    {
        public static IServiceCollection AddHttpClientJwt(this IServiceCollection services, IConfiguration configuration,
            string appsettingsPrefix = "")
        {
            services.Configure<HttpClientJwtOptions>(configuration.GetSection(appsettingsPrefix + HttpClientJwtOptions.ConfigurationSection));
            services.AddSingleton<AuthenticationDelegatingHandler>();
            services.AddHttpClient("sebcs", c =>
            {
                c.DefaultRequestHeaders.Add("Accept", "application/json");
            }).AddHttpMessageHandler<AuthenticationDelegatingHandler>();

            return services;
        }
    }
}

