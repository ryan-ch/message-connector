
namespace XB.Kafka
{
    public interface IKafkaConsumer
    {
        string Consume(string topic);
    }
}
