using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace XB.IBM.MQ
{
    public class MqProducer : MqBase<MqProducer>, IMqProducer
    {
        public MqProducer(ILogger<MqProducer> logger, IConfiguration configuration)
        : base(logger, configuration)
        {
        }

        ~MqProducer()
        {
            _destination?.Dispose();
            _connectionWmq?.Stop();
            _sessionWmq?.Close();
            _producer?.Close();
        }

        public override void Start()
        {
            base.Start();
            _producer = _sessionWmq.CreateProducer(_destination);
        }

        public void WriteMessage(string message)
        {
            var textMessage = _sessionWmq.CreateTextMessage();
            textMessage.Text = message;

            _producer.Send(textMessage);
        }
    }
}
