using Microsoft.Extensions.DependencyInjection;

namespace XB.IBM.MQ
{
    public static class MqClientExtensions
    {
        public static IServiceCollection AddMqClient(this IServiceCollection services)
        {
            services.AddTransient<IMqClient, MqClient>();

            return services;
        }
    }
}
