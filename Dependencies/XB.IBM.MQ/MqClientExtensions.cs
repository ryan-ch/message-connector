using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using XB.IBM.MQ.Config;
using XB.IBM.MQ.Implementations;
using XB.IBM.MQ.Interfaces;

namespace XB.IBM.MQ
{
    public static class MqClientExtensions
    {
        public static IServiceCollection AddMQ(this IServiceCollection services, IConfiguration configuration, string appsettingsPrefix = "")
        {
            services.Configure<MqOptions>(configuration.GetSection(appsettingsPrefix + MqOptions.ConfigurationSection));

            return services
                .AddMqConsumer()
                .AddMqProducer();// do we need this?
        }

        public static IServiceCollection AddMqConsumer(this IServiceCollection services)
        {
            return services.AddSingleton<IMqConsumer, MqConsumer>();
        }

        public static IServiceCollection AddMqProducer(this IServiceCollection services)
        {
            return services.AddSingleton<IMqProducer, MqProducer>();
        }
    }
}
