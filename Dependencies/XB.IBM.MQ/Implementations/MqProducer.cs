using IBM.XMS;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using XB.IBM.MQ.Config;
using XB.IBM.MQ.Interfaces;

namespace XB.IBM.MQ.Implementations
{
    public class MqProducer : MqBase, IMqProducer
    {
        public IMessageProducer Producer { get; }

        public MqProducer(IOptions<MqOptions> configurations, ILoggerFactory loggerFactory)
            : base(configurations.Value.WriterConfig, loggerFactory)
        {
            Producer = SessionWmq.CreateProducer(Destination);
        }

        public void WriteMessage(string message)
        {
            var textMessage = SessionWmq.CreateTextMessage();
            textMessage.Text = message;
            Producer.Send(textMessage);
        }
    }
}
