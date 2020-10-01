using System;
using IBM.XMS;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace XB.IBM.MQ
{
    public class MqConsumer : MqBase<MqConsumer>, IMqConsumer
    {
        public MqConsumer(ILogger<MqConsumer> logger, IConfiguration configuration)
        : base(logger, configuration)
        {
        }

        ~MqConsumer()
        {
            _destination.Dispose();
            _connectionWmq.Stop();
            _sessionWmq.Close();
            _consumer.Close();
        }

        public override void Start()
        {
            base.Start();
            _consumer = _sessionWmq.CreateConsumer(_destination);
            //_consumer.MessageListener = OnReceiveMessage;
        }

        public void OnReceiveMessage(IMessage message)
        {
            var mess = message as ITextMessage;
        }

        public string ReceiveMessage()
        {
            var message = _consumer.Receive() as ITextMessage;
            return message?.Text;
        }
    }
}
