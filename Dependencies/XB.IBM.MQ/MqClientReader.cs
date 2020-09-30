using System;
using IBM.XMS;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace XB.IBM.MQ
{
    public class MqClientReader : MqClientBase<MqClientReader>, IMqClientReader, IDisposable
    {

        private readonly ILogger<MqClientReader> _logger;

        public MqClientReader(ILogger<MqClientReader> logger, IConfiguration configuration)
        : base(logger, configuration)
        {
            _logger = logger;
        }

        public void Start()
        {
            _connectionWmq = _cf.CreateConnection();
            _sessionWmq = _connectionWmq.CreateSession(true, AcknowledgeMode.AutoAcknowledge);
            _destination = _sessionWmq.CreateQueue((string)_properties[XMSC.WMQ_QUEUE_NAME]);
            _connectionWmq.Start();
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
            _sessionWmq.Commit();
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
