using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using XB.Kafka;

namespace XB.Astrea.Client
{
    public static class AstreaClientExtensions
    {
        public static IServiceCollection AddAstreaClient(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient("astrea", c =>
            {
                c.BaseAddress = new Uri(configuration["AppSettings:Astrea:Url"]);
                c.DefaultRequestHeaders.Add("Accept", "text/plain");
            });

            services.AddTransient<IAstreaClient, AstreaClient>();
            //Todo: the Kafka injection shouldn't be here, or we move the kafka stuff to this project
            services.AddTransient<IKafkaProducer, KafkaProducer>();

            return services;
        }
    }
}
