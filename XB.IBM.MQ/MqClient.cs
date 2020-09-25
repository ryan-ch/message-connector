using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IBM.XMS;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace XB.IBM.MQ
{
    public class MqClient : IMqClient
    {
        private readonly XMSFactoryFactory _factoryFactory;
        private readonly IConnectionFactory _cf;
        private readonly IConnection _connectionWmq;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IConfiguration _configuration;
        private readonly IDictionary<string, object> _properties = new Dictionary<string, object>();
        private readonly ILogger<MqClient> _logger;

        private ISession _sessionWmq;
        private IDestination _destination;

        public MqClient(ILogger<MqClient> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
            _configuration = _serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IConfiguration>();
            _factoryFactory = XMSFactoryFactory.GetInstance(XMSC.CT_WMQ);
            _cf = _factoryFactory.CreateConnectionFactory();

            SetupProperties();
            SetupConnectionProperties();

            _connectionWmq = _cf.CreateConnection();
        }

        public void Start()
        {
            _sessionWmq = _connectionWmq.CreateSession(false, AcknowledgeMode.AutoAcknowledge);
            _destination = _sessionWmq.CreateQueue((string)_properties[XMSC.WMQ_QUEUE_NAME]);
            _connectionWmq.Start();
        }

        public async Task ReceiveMessageAsync(CancellationToken token)
        {
            await Task.Run(() =>
            {
                var consumer = _sessionWmq.CreateConsumer(_destination);

                ITextMessage message = (ITextMessage)consumer.Receive();

                _logger.LogInformation(message.Text);
            }, token);
        }

        public async Task WriteMessageAsync(string message, CancellationToken token)
        {
            await Task.Run(() =>
            {

                var producer = _sessionWmq.CreateProducer(_destination);

                var textMessage = _sessionWmq.CreateTextMessage();
                textMessage.Text = message;

                producer.Send(textMessage);
            }, token);
        }

        public void Stop()
        {
            _connectionWmq.Stop();
            _sessionWmq.Close();
        }


        private void SetupConnectionProperties()
        {
            _cf.SetStringProperty(XMSC.WMQ_HOST_NAME, (string)_properties[XMSC.WMQ_HOST_NAME]);
            _cf.SetIntProperty(XMSC.WMQ_PORT, Convert.ToInt32(_properties[XMSC.WMQ_PORT]));
            _cf.SetStringProperty(XMSC.WMQ_CHANNEL, (string)_properties[XMSC.WMQ_CHANNEL]);
            _cf.SetStringProperty(XMSC.WMQ_QUEUE_MANAGER, (string)_properties[XMSC.WMQ_QUEUE_MANAGER]);
            _cf.SetStringProperty(XMSC.WMQ_QUEUE_NAME, (string)_properties[XMSC.WMQ_QUEUE_NAME]);
            _cf.SetStringProperty(XMSC.USERID, (string)_properties[XMSC.USERID]);
            _cf.SetStringProperty(XMSC.PASSWORD, (string)_properties[XMSC.PASSWORD]);
        }

        private void SetupProperties()
        {
            _properties.Add(XMSC.WMQ_HOST_NAME, _configuration["AppSettings:MqHostnameReader"]);
            _properties.Add(XMSC.WMQ_PORT, _configuration["AppSettings:MqPortReader"]);
            _properties.Add(XMSC.WMQ_CHANNEL, _configuration["AppSettings:MqChannelReader"]);
            _properties.Add(XMSC.WMQ_QUEUE_MANAGER, _configuration["AppSettings:MqQueueManagerNameReader"]);
            _properties.Add(XMSC.WMQ_QUEUE_NAME, _configuration["AppSettings:MqQueueNameReader"]);
            _properties.Add(XMSC.USERID, _configuration["AppSettings:MqUserName"]);
            _properties.Add(XMSC.PASSWORD, _configuration["AppSettings:MqPassword"]);
        }
    }
}
