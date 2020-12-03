using IBM.XMS;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace XB.IBM.MQ.Implementations
{
    public class MqBase
    {
        public ISession SessionWmq { get; }
        public IDestination Destination { get; }
        private ILogger<MqBase> Logger { get; }

        public MqBase(IConfiguration configuration, string configurationSection, ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger<MqBase>();

            var properties = SetupProperties(configuration, configurationSection);

            //if (configuration[properties["section"] + "MqSslPath"] != "" && configuration[properties["section"] + "MqPassword"] != "")
            //{
            //    AddCertToCertStore(configuration[properties["section"] + "MqSslPath"], configuration[properties["section"] + "MqPassword"]);
            //}

            var connectionFactory = CreateAndConfigureConnectionFactory(properties);
            var connectionWmq = connectionFactory.CreateConnection();
            SessionWmq = connectionWmq.CreateSession(true, AcknowledgeMode.AutoAcknowledge);
            Destination = SessionWmq.CreateQueue(properties[XMSC.WMQ_QUEUE_NAME]);
            connectionWmq.Start();
        }

        public void Commit()
        {
            SessionWmq.Commit();
        }

        public void Rollback()
        {
            SessionWmq.Rollback();
        }

        private IConnectionFactory CreateAndConfigureConnectionFactory(IReadOnlyDictionary<string, string> properties)
        {
            var connectionFactory = XMSFactoryFactory.GetInstance(XMSC.CT_WMQ).CreateConnectionFactory();

            if (string.IsNullOrWhiteSpace(properties[XMSC.WMQ_SSL_CIPHER_SPEC]))
            {
                connectionFactory.SetStringProperty(XMSC.WMQ_SSL_CIPHER_SPEC, properties[XMSC.WMQ_SSL_CIPHER_SPEC]);
            }
            if (string.IsNullOrWhiteSpace(properties[XMSC.WMQ_SSL_KEY_REPOSITORY]))
            {
                connectionFactory.SetStringProperty(XMSC.WMQ_SSL_KEY_REPOSITORY, properties[XMSC.WMQ_SSL_KEY_REPOSITORY]);
            }
            if (string.IsNullOrWhiteSpace(properties[XMSC.WMQ_SSL_PEER_NAME]))
            {
                connectionFactory.SetStringProperty(XMSC.WMQ_SSL_PEER_NAME, properties[XMSC.WMQ_SSL_PEER_NAME]);
            }
            if (string.IsNullOrWhiteSpace(properties[XMSC.USERID]) && string.IsNullOrWhiteSpace(properties[XMSC.PASSWORD]))
            {
                connectionFactory.SetStringProperty(XMSC.USERID, properties[XMSC.USERID]);
                connectionFactory.SetStringProperty(XMSC.PASSWORD, properties[XMSC.PASSWORD]);
            }
            connectionFactory.SetStringProperty(XMSC.WMQ_HOST_NAME, properties[XMSC.WMQ_HOST_NAME]);
            connectionFactory.SetIntProperty(XMSC.WMQ_PORT, Convert.ToInt32(properties[XMSC.WMQ_PORT]));
            connectionFactory.SetStringProperty(XMSC.WMQ_CHANNEL, properties[XMSC.WMQ_CHANNEL]);
            connectionFactory.SetStringProperty(XMSC.WMQ_QUEUE_MANAGER, properties[XMSC.WMQ_QUEUE_MANAGER]);
            connectionFactory.SetStringProperty(XMSC.WMQ_QUEUE_NAME, properties[XMSC.WMQ_QUEUE_NAME]);
            connectionFactory.SetIntProperty(XMSC.WMQ_CONNECTION_MODE, XMSC.WMQ_CM_CLIENT);

            return connectionFactory;
        }

        private Dictionary<string, string> SetupProperties(IConfiguration configuration, string section) => new Dictionary<string, string>
            {
                {"section", section},
                {XMSC.WMQ_HOST_NAME, configuration[section + "MqHostname"]},
                {XMSC.WMQ_PORT, configuration[section + "MqPort"]},
                {XMSC.WMQ_CHANNEL, configuration[section + "MqChannel"]},
                {XMSC.WMQ_QUEUE_MANAGER, configuration[section + "MqQueueManagerName"]},
                {XMSC.WMQ_QUEUE_NAME, configuration[section + "MqQueueName"]},
                {XMSC.WMQ_SSL_CIPHER_SPEC, configuration[section + "MqSslCipher"]},
                {XMSC.WMQ_SSL_KEY_REPOSITORY, "*USER"},
                {XMSC.WMQ_SSL_PEER_NAME, configuration[section + "MqPeerName"]},
                {XMSC.USERID, configuration[section + "MqUserName"]},
                {XMSC.PASSWORD, configuration[section + "MqPassword"]}
            };

        //private void AddCertToCertStore(string certPath, string password)
        //{
        //    X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
        //    store.Open(OpenFlags.ReadWrite);

        //    X509Certificate2 cert = new X509Certificate2(certPath, password, X509KeyStorageFlags.UserKeySet);

        //    if (cert == null)
        //    {
        //        throw new ArgumentNullException("Unable to create certificate from provided arguments.");
        //    }

        //    store.Add(cert);
        //}
    }
}
