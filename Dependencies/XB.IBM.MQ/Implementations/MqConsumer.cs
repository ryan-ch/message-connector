using System.Collections.Generic;
using IBM.XMS;
using Microsoft.Extensions.Configuration;
using XB.IBM.MQ.Interfaces;

namespace XB.IBM.MQ.Implementations
{
    public class MqConsumer : IMqConsumer
    {
        public IMessageConsumer Consumer { get; }
        public MqBase MqBase { get; }
        public Dictionary<string, object> ConnectionProperties { get; }

        public MqConsumer(IConfiguration configuration)
        {
            ConnectionProperties = SetupProperties(configuration);
            MqBase = new MqBase(configuration, ConnectionProperties);
            Consumer = MqBase.SessionWmq.CreateConsumer(MqBase.Destination);
        }

        public string ReceiveMessage()
        {
            var message = Consumer.Receive() as ITextMessage;
            return message?.Text;
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
            const string section = "AppSettings:Reader:";
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
