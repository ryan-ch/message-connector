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
        private readonly IConnectionFactory _cf;
        private readonly IConfiguration _configuration;
        private readonly IDictionary<string, object> _properties = new Dictionary<string, object>();
        private readonly ILogger<MqClient> _logger;

        private IConnection _connectionWmq;
        private ISession _sessionWmq;
        private IDestination _destination;
        private IMessageConsumer _consumer;
        private IMessageProducer _producer;

        public MqClient(ILogger<MqClient> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _configuration = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IConfiguration>();
            var factoryFactory = XMSFactoryFactory.GetInstance(XMSC.CT_WMQ);
            _cf = factoryFactory.CreateConnectionFactory();

            SetupProperties();
            SetupConnectionProperties();
        }

        public void Start()
        {
            _connectionWmq = _cf.CreateConnection();
            _sessionWmq = _connectionWmq.CreateSession(true, AcknowledgeMode.AutoAcknowledge);
            _destination = _sessionWmq.CreateQueue((string)_properties[XMSC.WMQ_QUEUE_NAME]);
            _connectionWmq.Start();
            _producer = _sessionWmq.CreateProducer(_destination);
            _consumer = _sessionWmq.CreateConsumer(_destination);
        }

        public async Task<string> ReceiveMessageAsync(CancellationToken token)
        {
            return await Task.Run(() =>
            {
                ITextMessage message = (ITextMessage)_consumer.Receive();
                _sessionWmq.Commit();
                return message.Text;
            }, token);
        }

        public async Task WriteMessageAsync(string message, CancellationToken token)
        {
            await Task.Run(() =>
            {
                var textMessage = _sessionWmq.CreateTextMessage();
                textMessage.Text = message;

                _producer.Send(textMessage);
                _sessionWmq.Commit();
            }, token);
        }

        public void Stop()
        {
            _destination.Dispose();
            _connectionWmq.Stop();
            _sessionWmq.Close();
            _producer.Close();
            _consumer.Close();
        }


        private void SetupConnectionProperties()
        {
            //_cf.SetStringProperty(XMSC.WMQ_SSL_CIPHER_SPEC, (string)_properties[XMSC.WMQ_SSL_CIPHER_SPEC]);
            //_cf.SetStringProperty(XMSC.WMQ_SSL_KEY_REPOSITORY, (string)_properties[XMSC.WMQ_SSL_KEY_REPOSITORY]);
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
            //_properties.Add(XMSC.WMQ_SSL_CIPHER_SPEC, _configuration["AppSettings:MqSslCipherReader"]);
            //_properties.Add(XMSC.WMQ_SSL_KEY_REPOSITORY, _configuration["AppSettings:MqSslPathReader"]);
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
