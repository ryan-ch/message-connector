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
            string appsettingsPrefix = "", string httpClientIdentifier = "default")
        {
            var optionValue = configuration.GetSection(appsettingsPrefix + HttpClientJwtOptions.ConfigurationSection).Get<HttpClientJwtOptions>();
            var options = Options.Create<HttpClientJwtOptions>(optionValue);

            services.AddTransient<AuthenticationDelegatingHandler>();
            services.AddHttpClient(httpClientIdentifier, c =>
            {
                c.DefaultRequestHeaders.Add("Accept", "application/json");
            }).AddHttpMessageHandler(_ => new AuthenticationDelegatingHandler(options));

            return services;
        }
    }
}

