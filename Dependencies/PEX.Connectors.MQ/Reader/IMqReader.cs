using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PEX.Connectors.MQ.Reader
{
    public interface IMqReader : IDisposable
    {
        Task Start(string queueName, Dictionary<string, string> properties = null);
    }
}