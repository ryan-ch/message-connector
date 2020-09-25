using IBM.XMS;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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
