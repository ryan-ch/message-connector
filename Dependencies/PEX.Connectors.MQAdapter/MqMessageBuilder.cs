using IBM.WMQ;
using MQMessage = IBM.WMQAX.MQMessage;

namespace PEX.Connectors.MQAdapter
{
    public class MqMessageBuilder : IMqMessageBuilder
    {
        protected MQMessage MqMessage { get; set; }
        protected virtual string Format => MQC.MQFMT_STRING;

        public MQMessage Build(MqMessage message)
        {
            MqMessage = new MQMessage
            {
                Format = Format,
                CharacterSet = 1208,
                MessageIdString = message.MessageId,
                ApplicationIdData = message.ApplicationId
            };

            if (message.CorrelationId != null)
            {
                MqMessage.CorrelationId = message.CorrelationId.Value.ToByteArray();
            }

            foreach (var kvp in message.ExtendedProperties)
            {
                MqMessage.SetStringProperty(kvp.Key, kvp.Value);
            }

            SetSpecifics();

            MqMessage.WriteString(message.Data);

            return MqMessage;
        }

        protected virtual void SetSpecifics()
        {
        }
    }
}