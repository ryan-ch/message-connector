using IBM.XMS;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using XB.IBM.MQ.Config;
using XB.IBM.MQ.Interfaces;

namespace XB.IBM.MQ.Implementations
{
    public class MqConsumer : MqBase, IMqConsumer
    {
        public IMessageConsumer Consumer { get; }

        public MqConsumer(IOptions<MqOptions> configurations, ILoggerFactory loggerFactory)
            : base(configurations.Value.ReaderConfig, loggerFactory)
        {
            Consumer = SessionWmq.CreateConsumer(Destination);
        }

        public string ReceiveMessage()
        {
            var message = Consumer.Receive() as ITextMessage;
            return message?.Text;
        }
    }
}
