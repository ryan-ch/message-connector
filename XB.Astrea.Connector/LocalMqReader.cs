using System;
using Microsoft.Extensions.Logging;
using PEX.Connectors.MQ.Reader;
using PEX.Connectors.MQAdapter;
using PMC.Common.Wrappers;

namespace XB.Astrea.Connector
{
    public class LocalMqReader : MqReader
    {
        private readonly ILogger<LocalMqReader> _logger;
        public LocalMqReader(IMqQueueProcessor mqQueueProcessor, IConfigurationManagerWrapper configurationManagerWrapper, ILogger<LocalMqReader> logger) : base(mqQueueProcessor, configurationManagerWrapper)
        {
            _logger = logger;
        }

        public override void ProcessMessage(MqMessage message)
        {
            _logger.LogInformation(message.Data);
        }


    }
}
