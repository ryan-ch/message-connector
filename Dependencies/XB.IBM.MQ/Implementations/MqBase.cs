using IBM.XMS;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography.X509Certificates;
using XB.IBM.MQ.Config;

namespace XB.IBM.MQ.Implementations
{
    public class MqBase
    {
        protected ILogger<MqBase> Logger { get; }
        protected ISession SessionWmq { get; }
        protected IDestination Destination { get; }

        public MqBase(MqConfigurations configurations, ILogger<MqBase> logger, IConnectionFactory connectionFactory)
        {
            Logger = logger;
            ConfigureConnection(connectionFactory, configurations);
            var connectionWmq = connectionFactory.CreateConnection();
            SessionWmq = connectionWmq.CreateSession(true, AcknowledgeMode.AutoAcknowledge);
            Destination = SessionWmq.CreateQueue(configurations.MqQueueName);

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

        private void ConfigureConnection(IConnectionFactory connectionFactory, MqConfigurations config)
        {
            if (!string.IsNullOrWhiteSpace(config.MqSslPath) && !string.IsNullOrWhiteSpace(config.MqPassword))
            {
                AddCertificate(config.MqSslPath, config.MqPassword);
            }

            if (!string.IsNullOrWhiteSpace(config.MqSslCipher))
            {
                connectionFactory.SetStringProperty(XMSC.WMQ_SSL_CIPHER_SPEC, config.MqSslCipher);
            }

            if (!string.IsNullOrWhiteSpace(config.MqKeyRepo))
            {
                connectionFactory.SetStringProperty(XMSC.WMQ_SSL_KEY_REPOSITORY, config.MqKeyRepo);
            }

            if (!string.IsNullOrWhiteSpace(config.MqUsername) && !string.IsNullOrWhiteSpace(config.MqPassword))
            {
                connectionFactory.SetStringProperty(XMSC.USERID, config.MqUsername);
                connectionFactory.SetStringProperty(XMSC.PASSWORD, config.MqPassword);
            }

            connectionFactory.SetStringProperty(XMSC.WMQ_HOST_NAME, config.MqHostname);
            connectionFactory.SetIntProperty(XMSC.WMQ_PORT, config.MqPort);
            connectionFactory.SetStringProperty(XMSC.WMQ_CHANNEL, config.MqChannel);
            connectionFactory.SetStringProperty(XMSC.WMQ_QUEUE_MANAGER, config.MqQueueManagerName);
            connectionFactory.SetStringProperty(XMSC.WMQ_QUEUE_NAME, config.MqQueueName);
            connectionFactory.SetIntProperty(XMSC.WMQ_CONNECTION_MODE, XMSC.WMQ_CM_CLIENT);
            connectionFactory.SetIntProperty(XMSC.WMQ_CLIENT_RECONNECT_OPTIONS, XMSC.WMQ_CLIENT_RECONNECT);
            connectionFactory.SetIntProperty(XMSC.WMQ_CLIENT_RECONNECT_TIMEOUT, XMSC.WMQ_CLIENT_RECONNECT_TIMEOUT_DEFAULT);
            connectionFactory.SetIntProperty(XMSC.WMQ_RECEIVE_CONVERSION, XMSC.WMQ_RECEIVE_CONVERSION_QMGR);
            connectionFactory.SetIntProperty(XMSC.WMQ_RECEIVE_CCSID, 1208);
            connectionFactory.SetIntProperty(XMSC.WMQ_QMGR_CCSID, 278);
        }

        private void AddCertificate(string certPath, string password)
        {
            using var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadWrite);
            Logger.LogInformation($"Adding certificate to the store : {certPath}");
            var cert = new X509Certificate2(certPath, password, X509KeyStorageFlags.UserKeySet);
            store.Add(cert);
        }
    }
}
