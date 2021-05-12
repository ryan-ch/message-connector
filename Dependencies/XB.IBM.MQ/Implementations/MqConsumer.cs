using IBM.XMS;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Text;
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
            string messageText = message?.Text;
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Prod")
            {
                var mainFrameEncoder = CodePagesEncodingProvider.Instance.GetEncoding(Environment.GetEnvironmentVariable("EncodingFormat"));
                var bytes = mainFrameEncoder.GetBytes(message?.Text);
                messageText = Encoding.UTF8.GetString(bytes);
            }
            return messageText;
        }

        ~MqConsumer()
        {
            _consumer.Close();
        }
    }
}
