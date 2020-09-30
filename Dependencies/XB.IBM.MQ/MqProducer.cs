using System;
using IBM.XMS;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace XB.IBM.MQ
{
    public class MqProducer : MqBase<MqProducer>, IMqProducer, IDisposable
    {

        private readonly ILogger<MqProducer> _logger;

        public MqProducer(ILogger<MqProducer> logger, IConfiguration configuration)
        : base(logger, configuration)
        {
            _logger = logger;
        }

        public void Start()
        {
            _connectionWmq = _cf.CreateConnection();
            _sessionWmq = _connectionWmq.CreateSession(false, AcknowledgeMode.AutoAcknowledge);
            _destination = _sessionWmq.CreateQueue((string)_properties[XMSC.WMQ_QUEUE_NAME]);
            _connectionWmq.Start();
            _producer = _sessionWmq.CreateProducer(_destination);
        }

        public void WriteMessage(string message)
        {
            var textMessage = _sessionWmq.CreateTextMessage();
            textMessage.Text = message;

            _producer.Send(textMessage);
        }

        public void Dispose()
        {
            _destination.Dispose();
            _connectionWmq.Stop();
            _sessionWmq.Close();
            _producer.Close();
        }
    }
}
