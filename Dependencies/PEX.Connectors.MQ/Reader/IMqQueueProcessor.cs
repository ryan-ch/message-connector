using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PEX.Connectors.MQAdapter;

namespace PEX.Connectors.MQ.Reader
{
    public interface IMqQueueProcessor : IDisposable
    {
        Task Start(Action<MqMessage> proccessMessageFunc, IMqConnectionSettings connectionSettings, string queueName, Dictionary<string, string> properties = null);
    }
}