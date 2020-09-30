using Microsoft.Extensions.DependencyInjection;

namespace XB.IBM.MQ
{
    public static class MqClientExtensions
    {
        public static IServiceCollection AddMqClientReader(this IServiceCollection services)
        {
            services.AddTransient<IMqClientReader, MqClientReader>();

            return services;
        }

        public static IServiceCollection AddMqClientWriter(this IServiceCollection services)
        {
            services.AddTransient<IMqClientWriter, MqClientWriter>();

            return services;
        }
    }
}
