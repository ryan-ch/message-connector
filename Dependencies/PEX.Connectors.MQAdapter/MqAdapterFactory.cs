using System.Collections.Generic;

namespace PEX.Connectors.MQAdapter
{
    public class MqAdapterFactory : IMqAdapterFactory
    {
        private readonly IMqMessageBuilder _mqMessageBuilder;
        private readonly IMqQueueManagerFactory _mqQueueManagerFactory;

        public MqAdapterFactory(IMqMessageBuilder mqMessageBuilder, IMqQueueManagerFactory mqQueueManagerFactory)
        {
            _mqMessageBuilder = mqMessageBuilder;
            _mqQueueManagerFactory = mqQueueManagerFactory;
        }

        public IMqAdapter Create(Dictionary<string, string> properties)
        {
            return new MqAdapter(_mqMessageBuilder, _mqQueueManagerFactory, properties);
        }
    }
}