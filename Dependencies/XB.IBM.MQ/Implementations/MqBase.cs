using IBM.XMS;
using Microsoft.Extensions.Logging;
using XB.IBM.MQ.Config;

namespace XB.IBM.MQ.Implementations
{
    public class MqBase
    {
        public ISession SessionWmq { get; }
        public IDestination Destination { get; }
        private ILogger<MqBase> Logger { get; }

        public MqBase(MqConfigurations configurations, ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger<MqBase>();

            //if (configurations.MqSslPath != "" && configurations.MqPassword != "")
            //{
            //    AddCertToCertStore(configurations.MqSslPath, configurations.MqPassword);
            //}

            var connectionFactory = CreateAndConfigureConnectionFactory(configurations);
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

        private IConnectionFactory CreateAndConfigureConnectionFactory(MqConfigurations config)
        {
            var connectionFactory = XMSFactoryFactory.GetInstance(XMSC.CT_WMQ).CreateConnectionFactory();

            if (!string.IsNullOrWhiteSpace(config.MqSslCipher))
            {
                connectionFactory.SetStringProperty(XMSC.WMQ_SSL_CIPHER_SPEC, config.MqSslCipher);
            }

            // Todo: should this be assigned to SslPath?
            if (!string.IsNullOrWhiteSpace("*USER"))
            {
                connectionFactory.SetStringProperty(XMSC.WMQ_SSL_KEY_REPOSITORY, "*USER");
            }

            if (!string.IsNullOrWhiteSpace(config.MqPeerName))
            {
                connectionFactory.SetStringProperty(XMSC.WMQ_SSL_PEER_NAME, config.MqPeerName);
            }

            if (!string.IsNullOrWhiteSpace(config.MqUserName) && !string.IsNullOrWhiteSpace(config.MqPassword))
            {
                connectionFactory.SetStringProperty(XMSC.USERID, config.MqUserName);
                connectionFactory.SetStringProperty(XMSC.PASSWORD, config.MqPassword);
            }
            connectionFactory.SetStringProperty(XMSC.WMQ_HOST_NAME, config.MqHostname);
            connectionFactory.SetIntProperty(XMSC.WMQ_PORT, config.MqPort);
            connectionFactory.SetStringProperty(XMSC.WMQ_CHANNEL, config.MqChannel);
            connectionFactory.SetStringProperty(XMSC.WMQ_QUEUE_MANAGER, config.MqQueueManagerName);
            connectionFactory.SetStringProperty(XMSC.WMQ_QUEUE_NAME, config.MqQueueName);
            connectionFactory.SetIntProperty(XMSC.WMQ_CONNECTION_MODE, XMSC.WMQ_CM_CLIENT);

            return connectionFactory;
        }

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
