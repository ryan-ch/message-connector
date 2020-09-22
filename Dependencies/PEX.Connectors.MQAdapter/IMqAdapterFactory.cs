using System.Collections.Generic;

namespace PEX.Connectors.MQAdapter
{
    public interface IMqAdapterFactory
    {
        IMqAdapter Create(Dictionary<string, string> properties = null);
    }
}