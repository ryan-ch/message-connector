using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
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
        }

        public void Commit()
        {
            _sessionWmq.Commit();
        }

        public virtual void Start()
        {
            try
            {
                _connectionWmq = _cf.CreateConnection();
                _sessionWmq = _connectionWmq.CreateSession(true, AcknowledgeMode.AutoAcknowledge);
                _destination = _sessionWmq.CreateQueue((string) _properties[XMSC.WMQ_QUEUE_NAME]);
                _connectionWmq.Start();
            }
            catch (XMSException ex)
            {
                throw new InitiateMqClientException(ex.Message + ". Could not setup client");
            }
        }

        private void SetupConnectionProperties()
        {
            _cf.SetStringProperty(XMSC.WMQ_SSL_CIPHER_SPEC, (string)_properties[XMSC.WMQ_SSL_CIPHER_SPEC]);
            _cf.SetStringProperty(XMSC.WMQ_SSL_KEY_REPOSITORY, (string)_properties[XMSC.WMQ_SSL_KEY_REPOSITORY]);
            _cf.SetStringProperty(XMSC.WMQ_SSL_PEER_NAME, (string) _properties[XMSC.WMQ_SSL_PEER_NAME]);
            _cf.SetStringProperty(XMSC.WMQ_HOST_NAME, (string)_properties[XMSC.WMQ_HOST_NAME]);
            _cf.SetIntProperty(XMSC.WMQ_PORT, Convert.ToInt32(_properties[XMSC.WMQ_PORT]));
            _cf.SetStringProperty(XMSC.WMQ_CHANNEL, (string)_properties[XMSC.WMQ_CHANNEL]);
            _cf.SetStringProperty(XMSC.WMQ_QUEUE_MANAGER, (string)_properties[XMSC.WMQ_QUEUE_MANAGER]);
            _cf.SetStringProperty(XMSC.WMQ_QUEUE_NAME, (string)_properties[XMSC.WMQ_QUEUE_NAME]);
            _cf.SetIntProperty(XMSC.WMQ_CONNECTION_MODE, XMSC.WMQ_CM_CLIENT);

            _logger.LogInformation(_cf.GetStringProperty(XMSC.WMQ_QUEUE_MANAGER));
        }

        private void SetupProperties()
        {
            if (typeof(IMqConsumer).IsAssignableFrom(typeof(T)))
            {
                _properties.Add(XMSC.WMQ_SSL_CIPHER_SPEC, _configuration["AppSettings:MqSslCipherReader"]);
                _properties.Add(XMSC.WMQ_SSL_KEY_REPOSITORY, _configuration["AppSettings:MqSslPathReader"]);
                _properties.Add(XMSC.WMQ_HOST_NAME, _configuration["AppSettings:MqHostnameReader"]);
                _properties.Add(XMSC.WMQ_PORT, _configuration["AppSettings:MqPortReader"]);
                _properties.Add(XMSC.WMQ_CHANNEL, _configuration["AppSettings:MqChannelReader"]);
                _properties.Add(XMSC.WMQ_QUEUE_MANAGER, _configuration["AppSettings:MqQueueManagerNameReader"]);
                _properties.Add(XMSC.WMQ_QUEUE_NAME, _configuration["AppSettings:MqQueueNameReader"]);
            } else if (typeof(IMqProducer).IsAssignableFrom(typeof(T)))
            {
                _properties.Add(XMSC.WMQ_SSL_CIPHER_SPEC, _configuration["AppSettings:MqSslCipherWriter"]);
                _properties.Add(XMSC.WMQ_SSL_KEY_REPOSITORY, "*USER");
                _properties.Add(XMSC.WMQ_HOST_NAME, _configuration["AppSettings:MqHostnameWriter"]);
                _properties.Add(XMSC.WMQ_PORT, _configuration["AppSettings:MqPortWriter"]);
                _properties.Add(XMSC.WMQ_CHANNEL, _configuration["AppSettings:MqChannelWriter"]);
                _properties.Add(XMSC.WMQ_QUEUE_MANAGER, _configuration["AppSettings:MqQueueManagerNameWriter"]);
                _properties.Add(XMSC.WMQ_QUEUE_NAME, _configuration["AppSettings:MqQueueNameWriter"]);
            }

            AddCertToCertStore(_configuration["AppSettings:MqSslPathWriter"], _configuration["AppSettings:MqPassword"]);

            _properties.Add(XMSC.WMQ_SSL_PEER_NAME, _configuration["AppSettings:MqPeerNameWriter"]);
        }

        private void AddCertToCertStore(string certPath, string password)
        {
            X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadWrite);

            X509Certificate2 cert = new X509Certificate2(certPath, password, X509KeyStorageFlags.UserKeySet);

            _logger.LogInformation(certPath + " " + password);

            if (cert == null)
            {
                throw new ArgumentNullException("Unable to create certificate from provided arguments.");
            }

            store.Add(cert);
        }
    }
}
