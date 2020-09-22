using System.Collections;
using IBM.WMQ;

namespace PEX.Connectors.MQAdapter
{
    public interface IMqQueueManagerFactory
    {
        MQQueueManager Create(string settingsQueueManagerName, Hashtable properties);
    }

    public class MqQueueManagerFactory : IMqQueueManagerFactory
    {
        private MQQueueManager _mqQueueManager;

        public MQQueueManager Create(string queueManagerName, Hashtable properties)
        {
            if (_mqQueueManager == null || !_mqQueueManager.IsConnected)
            {
                _mqQueueManager = new MQQueueManager(queueManagerName, properties);
            }

            return _mqQueueManager;
        }

    }
}