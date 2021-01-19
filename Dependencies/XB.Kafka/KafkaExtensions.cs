using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XB.Kafka.Config;

namespace XB.Kafka
{
    public static class KafkaExtensions
    {
        public static IServiceCollection AddKafkaProducer(this IServiceCollection services, IConfiguration configuration, string appsettingsPrefix = "")
        {
            services.Configure<KafkaProducerConfig>(configuration.GetSection(appsettingsPrefix + KafkaProducerConfig.ConfigurationSection));
            return services.AddScoped<IKafkaProducer, KafkaProducer>();
        }

        public static IServiceCollection AddKafkaConsumer(this IServiceCollection services, IConfiguration configuration, string appsettingsPrefix = "")
        {
            services.Configure<KafkaConsumerConfig>(configuration.GetSection(appsettingsPrefix + KafkaConsumerConfig.ConfigurationSection));
            return services.AddScoped<IKafkaConsumer, KafkaConsumer>();
        }
    }
}
