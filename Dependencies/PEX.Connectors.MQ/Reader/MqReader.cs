using System.Collections.Generic;
using System.Threading.Tasks;
using PEX.Connectors.MQAdapter;
using PMC.Common.Wrappers;

namespace PEX.Connectors.MQ.Reader
{
    public abstract class MqReader : IMqReader
    {
        private readonly IMqQueueProcessor _mqQueueProcessor;
        private readonly IConfigurationManagerWrapper _configurationManagerWrapper;

        protected MqReader(IMqQueueProcessor mqQueueProcessor, IConfigurationManagerWrapper configurationManagerWrapper)
        {
            _mqQueueProcessor = mqQueueProcessor;
            _configurationManagerWrapper = configurationManagerWrapper;
        }

        public Task Start(string queueName, Dictionary<string, string> properties)
        {
            var mqConnectionSettings = GetMqSettings();
            return _mqQueueProcessor.Start(ProcessMessage, mqConnectionSettings, queueName, properties);
        }

        public abstract void ProcessMessage(MqMessage message);

        private IMqConnectionSettings GetMqSettings(string queueName = "")
        {
            return new MqConnectionSettings
            {
                Channel = _configurationManagerWrapper.Configuration["AppSettings:MqChannelReader"],
                QueueManagerName = _configurationManagerWrapper.Configuration["AppSettings:MqQueueManagerNameReader"],
                Hostname = _configurationManagerWrapper.Configuration["AppSettings:MqHostnameReader"],
                Port = int.Parse(_configurationManagerWrapper.Configuration["AppSettings:MqPortReader"]),
                PollInterval = int.Parse(_configurationManagerWrapper.Configuration["AppSettings:MqPollIntervalReader"]),
                SslPath = _configurationManagerWrapper.Configuration["AppSettings:MqSslPathReader"],
                SslChyper = _configurationManagerWrapper.Configuration["AppSettings:MqSslChyperReader"],
                UserName = _configurationManagerWrapper.Configuration["AppSettings:MqUserName"],
                Password = _configurationManagerWrapper.Configuration["AppSettings:MqPassword"]
            };
        }

        public void Dispose()
        {
            _mqQueueProcessor.Dispose();
        }
     
    }
}