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
                c.BaseAddress = new Uri("http://astrea-swift-fcp-sys.k8-nifi.sebank.se");
                c.DefaultRequestHeaders.Add("Accept", "text/plain");
            });

            services.AddTransient<IAstreaClient, AstreaClient>();

            return services;
        }
    }
}
