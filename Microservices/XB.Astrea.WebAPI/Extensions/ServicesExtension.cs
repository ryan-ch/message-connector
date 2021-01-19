using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using XB.Astrea.Client;
using XB.Hubert;
using XB.IBM.MQ;
using XGalaxy.Common.ExceptionHandling;
using XGalaxy.Common.Logging;
using XGalaxy.Common.Logging.Contracts;

namespace XB.Astrea.WebAPI.Extensions
{
    public static class ServicesExtension
    {
        private const string appsettingsPrefix = "AppSettings:";

        public static void ConfigureDependencies(this IServiceCollection services, IConfiguration config)
        {
            services.AddAstreaClient(config, appsettingsPrefix);
            services.AddMQ(config, appsettingsPrefix);
            services.AddHubert();
        }

        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<ILoggingService, LoggingService>();
        }

        public static void ConfigureCustomExceptionHandler(this IApplicationBuilder app, ILoggingService loggingService)
        {
            app.UseExceptionHandler(app => app.Run(async context => await ExceptionMiddleware.HandleExceptionAsync(context, loggingService)));
        }
    }
}
