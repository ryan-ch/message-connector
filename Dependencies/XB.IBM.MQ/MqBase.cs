using System;
using System.Collections.Generic;
using IBM.XMS;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace XB.IBM.MQ
{
    public class MqBase<T>
    {
        protected readonly IConfiguration _configuration;
        protected readonly ILogger<T> _logger;
        protected readonly IConnectionFactory _cf;
        protected IDictionary<string, object> _properties = new Dictionary<string, object>();
        protected IConnection _connectionWmq;
        protected ISession _sessionWmq;
        protected IDestination _destination;
        protected IMessageConsumer _consumer;
        protected IMessageProducer _producer;

        public MqBase(ILogger<T> logger, IConfiguration configuration)
        {
            _configuration = configuration;
            _logger = logger;
            var factoryFactory = XMSFactoryFactory.GetInstance(XMSC.CT_WMQ);
            _cf = factoryFactory.CreateConnectionFactory();

            SetupProperties();
            SetupConnectionProperties();

            Start();
        }

        public void Commit()
        {
            _sessionWmq.Commit();
        }

        public virtual void Start()
        {
            _connectionWmq = _cf.CreateConnection();
            _sessionWmq = _connectionWmq.CreateSession(true, AcknowledgeMode.AutoAcknowledge);
            _destination = _sessionWmq.CreateQueue((string)_properties[XMSC.WMQ_QUEUE_NAME]);
            _connectionWmq.Start();
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
            _cf.SetIntProperty(XMSC.WMQ_CONNECTION_MODE, XMSC.WMQ_CM_CLIENT);
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
