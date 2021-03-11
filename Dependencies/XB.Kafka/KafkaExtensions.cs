using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using XB.Kafka.Config;

namespace XB.Kafka
{
    public static class KafkaExtensions
    {
        public static IServiceCollection AddKafkaProducer(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<KafkaProducerConfig>(configuration.GetSection(KafkaProducerConfig.ConfigurationSection));
            return services.AddSingleton<IKafkaProducer, KafkaProducer>();
        }

        public static IServiceCollection AddKafkaConsumer(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<KafkaConsumerConfig>(configuration.GetSection(KafkaConsumerConfig.ConfigurationSection));
            return services.AddSingleton<IKafkaConsumer, KafkaConsumer>();
        }
    }
}
