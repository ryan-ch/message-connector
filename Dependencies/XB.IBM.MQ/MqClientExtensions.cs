using Microsoft.Extensions.DependencyInjection;
using XB.IBM.MQ.Implementations;
using XB.IBM.MQ.Interfaces;

namespace XB.IBM.MQ
{
    public static class MqClientExtensions
    {
        public static IServiceCollection AddMqConsumer(this IServiceCollection services)
        {
            services.AddSingleton<IMqConsumer, MqConsumer>();

            return services;
        }

        public static IServiceCollection AddMqProducer(this IServiceCollection services)
        {
            services.AddSingleton<IMqProducer, MqProducer>();

            return services;
        }
    }
}
