using System;

namespace PEX.Connectors.MQAdapter
{
    public interface IMqAdapter : IDisposable
    {
        bool IsConnected { get; }
        IMqConnectionSettings MqConnectionSettings { get; }
        void Put(MqMessage message, string queueName);
        MqMessage Get(string applicationId, string queueName);
        void Connect(IMqConnectionSettings settings);
        void Commit();
        void Backout();
        int GetQueueDepth(string queueName);
    }
}