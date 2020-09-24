using System;
using System.Threading.Tasks;
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
        private readonly IAstreaClient _astreaClient;

        public LocalMqReader(IMqQueueProcessor mqQueueProcessor, IConfigurationManagerWrapper configurationManagerWrapper, ILogger<LocalMqReader> logger, IAstreaClient astreaClient) : base(mqQueueProcessor, configurationManagerWrapper)
        {
            _logger = logger;
            _astreaClient = astreaClient;
        }

        public override async Task ProcessMessage(MqMessage message)
        {
            _logger.LogInformation(message.Data);
            string result = await _astreaClient.SayHelloAsync();
            _logger.LogInformation(result);
        }
    }
}
