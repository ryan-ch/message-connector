using System;
using Microsoft.Extensions.DependencyInjection;

namespace XB.Astrea
{
    public static class AstreaClientExtensions
    {
        public static IServiceCollection AddAstreaClient(this IServiceCollection services)
        {
            services.AddHttpClient("astrea", c =>
            {
                c.BaseAddress = new Uri("https://assess-service-fcp-astrea-dev.cumulus.sebank.se");
                c.DefaultRequestHeaders.Add("Accept", "text/plain");
            });

            services.AddTransient<IAstreaClient, AstreaClient>();

            return services;
        }
    }
}
