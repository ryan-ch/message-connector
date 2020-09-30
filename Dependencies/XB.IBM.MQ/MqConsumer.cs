using System;
using IBM.XMS;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace XB.IBM.MQ
{
    public class MqConsumer : MqBase<MqConsumer>, IMqConsumer, IDisposable
    {

        private readonly ILogger<MqConsumer> _logger;

        public MqConsumer(ILogger<MqConsumer> logger, IConfiguration configuration)
        : base(logger, configuration)
        {
            _logger = logger;
        }

        public void Start()
        {
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

        public void Dispose()
        {
            _destination.Dispose();
            _connectionWmq.Stop();
            _sessionWmq.Close();
            _consumer.Close();
        }
    }
}
