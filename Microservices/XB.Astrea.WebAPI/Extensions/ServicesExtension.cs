using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using XB.Astrea.Client;
using XB.IBM.MQ;
using XB.MtParser;

namespace XB.Astrea.WebAPI.Extensions
{
    public static class ServicesExtension
    {
        public static void ConfigureDependencies(this IServiceCollection services, IConfiguration config)
        {
            services.AddAstreaClient(config);
            services.AddMq(config);
            services.AddMtParser();
        }
    }
}
