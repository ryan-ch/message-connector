using System;
using PEX.Connectors.MQAdapter;

namespace PEX.Connectors.MQ.Reader
{
    public interface IMqQueuePoller
    {
        void Poll(IMqAdapter mqAdapter, Action<MqMessage> proccessMessageFunc, string queueName);
    }
}