using IBM.XMS;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using XB.IBM.MQ.Interfaces;

namespace XB.IBM.MQ.Implementations
{
    public class MqConsumer : MqBase, IMqConsumer
    {
        //Todo: add MQ identifier to the section
        private const string section = "AppSettings:Reader:";

        public IMessageConsumer Consumer { get; }

        public MqConsumer(IConfiguration configuration, ILoggerFactory loggerFactory)
            : base(configuration, section, loggerFactory)
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
