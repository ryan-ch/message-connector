using System.Collections.Generic;
using IBM.XMS;
using Microsoft.Extensions.Configuration;
using XB.IBM.MQ.Interfaces;

namespace XB.IBM.MQ.Implementations
{
    public class MqProducer : IMqProducer
    {
        public IMessageProducer Producer { get; }
        public IConfiguration Configuration { get; }
        public MqBase MqBase { get; }
        public Dictionary<string, object> ConnectionProperties { get; }

        public MqProducer(IConfiguration configuration)
        {
            Configuration = configuration;
            ConnectionProperties = SetupProperties(Configuration);
            MqBase = new MqBase(Configuration, ConnectionProperties);
            Producer = MqBase.SessionWmq.CreateProducer(MqBase.Destination);
        }

        public void WriteMessage(string message)
        {
            var textMessage = MqBase.SessionWmq.CreateTextMessage();
            textMessage.Text = message;

            Producer.Send(textMessage);
        }

        public void Commit()
        {
            MqBase.Commit();
        }

        public void Rollback()
        {
            MqBase.Rollback();
        }

        private Dictionary<string, object> SetupProperties(IConfiguration configuration)
        {
            const string section = "AppSettings:Writer:";
            return new Dictionary<string, object>
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
        }
    }
}
