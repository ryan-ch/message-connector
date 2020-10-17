using Microsoft.Extensions.DependencyInjection;

namespace XB.IBM.MQ
{
    public static class MqFactoryExtensions
    {
        public static IServiceCollection AddMqFactory(this IServiceCollection services)
        {
            services.AddMqConsumer();
            services.AddMqProducer();

            return services;
        }
    }
}
