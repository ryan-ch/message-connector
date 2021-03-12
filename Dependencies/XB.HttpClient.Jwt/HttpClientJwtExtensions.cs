﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using XB.HttpClientJwt.Config;

namespace XB.HttpClientJwt
{
    public static class HttpClientJwtExtensions
    {
        public static IHttpClientBuilder AddHttpClientJwt(this IServiceCollection services, IConfiguration configuration,
            string httpClientIdentifier = "default")
        {
            return services
                 .Configure<HttpClientJwtOptions>(configuration.GetSection(HttpClientJwtOptions.ConfigurationSection))
                 .AddTransient<AuthenticationDelegatingHandler>()
                 .AddHttpClient(httpClientIdentifier, c => { c.DefaultRequestHeaders.Add("Accept", "application/json"); })
                 .AddHttpMessageHandler<AuthenticationDelegatingHandler>();
        }
    }
}

