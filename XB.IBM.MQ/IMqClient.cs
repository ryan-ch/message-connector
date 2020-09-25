using System.Threading;
using System.Threading.Tasks;

namespace XB.IBM.MQ
{
    public interface IMqClient
    {
        void Start();

        Task ReceiveMessageAsync(CancellationToken token);

        Task WriteMessageAsync(string message, CancellationToken token);

        void Stop();
    }
}
