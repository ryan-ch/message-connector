using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using XB.Astrea.WebAPI.Extensions;
using XGalaxy.Common.Logging;

namespace XB.Astrea.WebAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _ = bool.TryParse(Environment.GetEnvironmentVariable("Debugging"), out var _debugging);
            SerilogConfiguration.ConfigureLogging("Astrea-Connector", _debugging);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureDependencies(Configuration);
            services.AddControllers();
            services.AddHostedService<Worker>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
