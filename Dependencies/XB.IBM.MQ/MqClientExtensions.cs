using IBM.XMS;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using XB.IBM.MQ.Config;
using XB.IBM.MQ.Implementations;
using XB.IBM.MQ.Interfaces;

namespace XB.IBM.MQ
{
    public static class MqClientExtensions
    {
        public static IServiceCollection AddMq(this IServiceCollection services, IConfiguration configuration)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            services.Configure<MqOptions>(configuration.GetSection(MqOptions.ConfigurationSection));
            return services
                .AddSingleton(_ => XMSFactoryFactory.GetInstance(XMSC.CT_WMQ).CreateConnectionFactory())
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
