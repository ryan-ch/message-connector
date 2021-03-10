using IBM.XMS;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using XB.IBM.MQ.Config;
using XB.IBM.MQ.Interfaces;

namespace XB.IBM.MQ.Implementations
{
    public class MqConsumer : MqBase, IMqConsumer
    {
        private readonly IMessageConsumer _consumer;

        public MqConsumer(IOptions<MqOptions> configurations, ILogger<MqConsumer> logger, IConnectionFactory connectionFactory)
            : base(configurations.Value.ReaderConfig, logger, connectionFactory)
        {
            _consumer = SessionWmq.CreateConsumer(Destination);
        }

        public string ReceiveMessage(long waitTimeMs = 0)
        {
            var message = _consumer.Receive(waitTimeMs) as ITextMessage;
            return message?.Text;
        }

        ~MqConsumer()
        {
            _consumer.Close();
        }
    }
}
