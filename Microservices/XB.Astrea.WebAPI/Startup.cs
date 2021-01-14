using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using XB.Astrea.WebAPI.Extensions;
using XGalaxy.Common.Logging.Contracts;
using XGalaxy.Common.Logging.Helpers;

namespace XB.Astrea.WebAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            new LoggingHelper("Astrea-Connector").ConfigureLogging();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureDependencies(Configuration);
            services.ConfigureServices();
            services.AddControllers();
            //Todo: does this actually start the service or should we start it manually?
            services.AddHostedService<Worker>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggingService loggingService)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.ConfigureCustomExceptionHandler(loggingService);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
