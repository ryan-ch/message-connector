using System;
using System.Collections.Generic;

namespace PEX.Connectors.MQAdapter
{
    public class MqMessage
    {
        public MqMessage(string data) : this(data, string.Empty)
        {
        }

        public MqMessage(string data, string applicationId) : this(data, applicationId, 0)
        {
        }

        public MqMessage(string data, string applicationId, int backoutCount)
        {
            Data = data;
            ApplicationId = applicationId;
            BackoutCount = backoutCount;
            UseUnixEncoding = false;
            MessageId = string.Empty;
            CorrelationId = null;
            Properties = null;
            ExtendedProperties = new Dictionary<string, string>();
        }

        public string MessageId { get; set; }
        public Guid? CorrelationId { get; set; }
        public int BackoutCount { get; set; }
        public bool UseUnixEncoding { get; set; }
        public string Data { get; }
        public string ApplicationId { get; }
        public Dictionary<string, string> Properties { get; set; }
        public Dictionary<string, string> ExtendedProperties { get; set; }

        public override string ToString()
        {
            return $"{ApplicationId}:{Data}";
        }
    }
}