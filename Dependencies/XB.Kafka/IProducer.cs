using System.Threading.Tasks;

namespace XB.Kafka
{
    public interface IProducer
    {
        Task Execute(string message);
    }
}
