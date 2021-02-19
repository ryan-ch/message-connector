using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using XB.Astrea.Client;
using XB.Hubert;
using XB.IBM.MQ;
using XB.MtParser;

namespace XB.Astrea.WebAPI.Extensions
{
    public static class ServicesExtension
    {
        private const string appsettingsPrefix = "AppSettings:";

        public static void ConfigureDependencies(this IServiceCollection services, IConfiguration config)
        {
            services.AddAstreaClient(config, appsettingsPrefix);
            services.AddMQ(config, appsettingsPrefix);
            services.AddMtParser();
        }
    }
}
