using System.Threading.Tasks;

namespace XB.Kafka
{
    public interface IKafkaProducer
    {
        Task Execute(string message);
    }
}
