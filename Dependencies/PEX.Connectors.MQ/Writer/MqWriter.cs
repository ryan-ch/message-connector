using PEX.Connectors.MQAdapter;
using PMC.Common.Wrappers;

namespace PEX.Connectors.MQ.Writer
{
    public class MqWriter : IMqWriter
    {
        private readonly IConfigurationManagerWrapper _configurationManagerWrapper;
        private readonly IMqAdapter _mqAdapter;

        public MqWriter(IMqAdapterFactory mqAdapterFactory, IConfigurationManagerWrapper configurationManagerWrapper)
        {
            _configurationManagerWrapper = configurationManagerWrapper;
            _mqAdapter = mqAdapterFactory.Create();
        }

        public void Write(MqMessage message, string queueName)
        {
            var mqConnectionSettings = GetMqSettings();
            ConnectAndPutOnMq(message, mqConnectionSettings, queueName);
        }

        private void ConnectAndPutOnMq(MqMessage message, IMqConnectionSettings mqConnectionSettings, string queueName)
        {
            _mqAdapter.Connect(mqConnectionSettings);
            _mqAdapter.Put(message, queueName);
        }

        public void Dispose()
        {
            _mqAdapter.Dispose();
        }

        private IMqConnectionSettings GetMqSettings()
        {
            return new MqConnectionSettings
            {
                Channel = _configurationManagerWrapper.Configuration["AppSettings:MqChannelWriter"],
                QueueManagerName = _configurationManagerWrapper.Configuration["AppSettings:MqQueueManagerNameWriter"],
                Hostname = _configurationManagerWrapper.Configuration["AppSettings:MqHostnameWriter"],
                Port = int.Parse(_configurationManagerWrapper.Configuration["AppSettings:MqPortWriter"]),
                PollInterval = int.Parse(_configurationManagerWrapper.Configuration["AppSettings:MqPollIntervalWriter"]),
                SslPath = _configurationManagerWrapper.Configuration["AppSettings:MqSslPathWriter"],
                SslChyper = _configurationManagerWrapper.Configuration["AppSettings:MqSslChyperWriter"],
                UserName = _configurationManagerWrapper.Configuration["AppSettings:MqUserName"],
                Password = _configurationManagerWrapper.Configuration["AppSettings:MqPassword"]
            };
        }
    }
}