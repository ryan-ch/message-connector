using System;
using PEX.Connectors.MQAdapter;

namespace PEX.Connectors.MQ.Reader
{
    public class MqQueuePoller : IMqQueuePoller
    {
        public void Poll(IMqAdapter mqAdapter, Action<MqMessage> proccessMessageFunc, string queueName)
        {
            MqMessage message;

            do
            {
                message = mqAdapter.Get(string.Empty, queueName);
                if (message != null)
                {
                    try
                    {
                        proccessMessageFunc(message);
                        mqAdapter.Commit();
                    }
                    catch (Exception e)
                    {
                        mqAdapter.Backout();
                        throw;
                    }
                }
            } while (message != null);
        }
    }
}