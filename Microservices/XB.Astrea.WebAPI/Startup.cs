using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
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
            _ = bool.TryParse(Environment.GetEnvironmentVariable("Debugging"), out var debugging);
            var config = new LoggerConfiguration()
                .Enrich.WithMachineName()
                .Enrich.WithExceptionDetails()
                .WriteTo.File("C:/logs/astrea-connector-logs.txt", restrictedToMinimumLevel: LogEventLevel.Error, outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}] Level:{Level:u3} Message:{Message:lj} MachineName:{MachineName} {NewLine}{Exception}{NewLine}")
                .WriteTo.Console(outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}] Level:{Level:u3} Message:{Message:lj} MachineName:{MachineName} {NewLine}{Exception}{NewLine}");

            Log.Logger = config.CreateLogger();
            //SerilogConfiguration.ConfigureLogging("Astrea-Connector", debugging);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
           {
               c.SwaggerDoc("v1", new OpenApiInfo { Title = "Astrea Connector", Version = "v1" });
           });

            services.ConfigureDependencies(Configuration);
            services.AddControllers();
            services.AddSingleton<BackgroundServicesManager>();
            services.AddHostedService<Worker>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Astrea Connector"));

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
