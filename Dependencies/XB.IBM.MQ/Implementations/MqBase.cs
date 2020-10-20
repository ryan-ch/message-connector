using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using IBM.XMS;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace XB.IBM.MQ.Implementations
{
    public class MqBase
    {
        public IConnectionFactory Cf { get; }
        public IConnection ConnectionWmq { get; }
        public ISession SessionWmq { get; }
        public IDestination Destination { get; }
        protected IConfiguration Configuration { get; }
        private ILogger<MqBase> Logger { get; }

        public MqBase(IConfiguration configuration, IReadOnlyDictionary<string, object> properties, ILogger<MqBase> logger)
        {
            Configuration = configuration;
            Logger = logger;
            var factoryFactory = XMSFactoryFactory.GetInstance(XMSC.CT_WMQ);
            Cf = factoryFactory.CreateConnectionFactory();

            SetupConnectionProperties(properties);

            if (Configuration[properties["section"] + "MqSslPath"] != "" && Configuration[properties["section"] + "MqPassword"] != "")
            {
                AddCertToCertStore(Configuration[properties["section"] + "MqSslPath"], Configuration[properties["section"] + "MqPassword"]);
            }

            ConnectionWmq = Cf.CreateConnection();
            SessionWmq = ConnectionWmq.CreateSession(true, AcknowledgeMode.AutoAcknowledge);
            Destination = SessionWmq.CreateQueue((string)properties[XMSC.WMQ_QUEUE_NAME]);
            Logger.LogInformation("Starting Mq Connection");
            ConnectionWmq.Start();
        }

        public void Commit()
        {
            SessionWmq.Commit();
        }

        public void Rollback()
        {
            SessionWmq.Rollback();
        }

        private void SetupConnectionProperties(IReadOnlyDictionary<string, object> properties)
        {
            if ((string)properties[XMSC.WMQ_SSL_CIPHER_SPEC] != "")
            {
                Cf.SetStringProperty(XMSC.WMQ_SSL_CIPHER_SPEC, (string)properties[XMSC.WMQ_SSL_CIPHER_SPEC]);
            }
            if ((string)properties[XMSC.WMQ_SSL_KEY_REPOSITORY] != "")
            {
                Cf.SetStringProperty(XMSC.WMQ_SSL_KEY_REPOSITORY, (string)properties[XMSC.WMQ_SSL_KEY_REPOSITORY]);
            }
            if ((string)properties[XMSC.WMQ_SSL_PEER_NAME] != "") {
                Cf.SetStringProperty(XMSC.WMQ_SSL_PEER_NAME, (string)properties[XMSC.WMQ_SSL_PEER_NAME]);
            }
            if ((string)properties[XMSC.USERID] != "" && (string)properties[XMSC.PASSWORD] != "")
            {
                Cf.SetStringProperty(XMSC.USERID, (string)properties[XMSC.USERID]);
                Cf.SetStringProperty(XMSC.PASSWORD, (string)properties[XMSC.PASSWORD]);
            }
            Cf.SetStringProperty(XMSC.WMQ_HOST_NAME, (string)properties[XMSC.WMQ_HOST_NAME]);
            Cf.SetIntProperty(XMSC.WMQ_PORT, Convert.ToInt32(properties[XMSC.WMQ_PORT]));
            Cf.SetStringProperty(XMSC.WMQ_CHANNEL, (string)properties[XMSC.WMQ_CHANNEL]);
            Cf.SetStringProperty(XMSC.WMQ_QUEUE_MANAGER, (string)properties[XMSC.WMQ_QUEUE_MANAGER]);
            Cf.SetStringProperty(XMSC.WMQ_QUEUE_NAME, (string)properties[XMSC.WMQ_QUEUE_NAME]);
            Cf.SetIntProperty(XMSC.WMQ_CONNECTION_MODE, XMSC.WMQ_CM_CLIENT);
        }

        private void AddCertToCertStore(string certPath, string password)
        {
            X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadWrite);

            X509Certificate2 cert = new X509Certificate2(certPath, password, X509KeyStorageFlags.UserKeySet);

            if (cert == null)
            {
                throw new ArgumentNullException("Unable to create certificate from provided arguments.");
            }

            store.Add(cert);
        }
    }
}
