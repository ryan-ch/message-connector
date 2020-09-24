using System;
using Microsoft.Extensions.Logging;
using PEX.Connectors.MQ.Reader;
using PEX.Connectors.MQAdapter;
using PMC.Common.Logging;
using PMC.Common.Wrappers;
using LogLevel = PMC.Common.Logging.LogLevel;

namespace XB.Astrea.Connector
{
    public class LocalMqReader : MqReader
    {
        private readonly ILogger<LocalMqReader> _logger;
        private readonly IClientLogger _clientLogger;

        public LocalMqReader(IMqQueueProcessor mqQueueProcessor, IConfigurationManagerWrapper configurationManagerWrapper, ILogger<LocalMqReader> logger, IClientLogger clientLogger) : base(mqQueueProcessor, configurationManagerWrapper)
        {
            _logger = logger;
            _clientLogger = clientLogger;
        }

        public override void ProcessMessage(MqMessage message)
        {
            _logger.LogInformation(message.Data);
            _clientLogger.Log("", LogLevel.Info);
        }


    }
}
