using System;
using PEX.Connectors.MQAdapter;

namespace PEX.Connectors.MQ.Writer
{
    public interface IMqWriter : IDisposable
    {
        void Write(MqMessage message, string queueName);
    }
}
