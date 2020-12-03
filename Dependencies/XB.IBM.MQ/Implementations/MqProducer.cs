using IBM.XMS;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using XB.IBM.MQ.Interfaces;

namespace XB.IBM.MQ.Implementations
{
    public class MqProducer : MqBase, IMqProducer
    {
        //Todo: add MQ identifier to the section
        private const string section = "AppSettings:Writer:";
        public IMessageProducer Producer { get; }

        public MqProducer(IConfiguration configuration, ILoggerFactory loggerFactory)
            : base(configuration, section, loggerFactory)
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
