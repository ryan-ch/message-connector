using Microsoft.Extensions.DependencyInjection;

namespace XB.IBM.MQ
{
    public static class MqClientExtensions
    {
        public static IServiceCollection AddMqConsumer(this IServiceCollection services)
        {
            services.AddTransient<IMqConsumer, MqConsumer>();

            return services;
        }

        public static IServiceCollection AddMqProducer(this IServiceCollection services)
        {
            services.AddTransient<IMqProducer, MqProducer>();

            return services;
        }
    }
}
