using IBM.XMS;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using XB.IBM.MQ.Config;
using XB.IBM.MQ.Interfaces;

namespace XB.IBM.MQ.Implementations
{
    public class MqProducer : MqBase, IMqProducer
    {
        private readonly IMessageProducer _producer;

        public MqProducer(IOptions<MqOptions> configurations, ILogger<MqProducer> logger, IConnectionFactory connectionFactory)
            : base(configurations.Value.WriterConfig, logger, connectionFactory)
        {
            _producer = SessionWmq.CreateProducer(Destination);
        }

        public void WriteMessage(string message)
        {
            var textMessage = SessionWmq.CreateTextMessage(message);
            _producer.Send(textMessage);
        }

        ~MqProducer()
        {
            _producer.Close();
        }
    }
}
