using IBM.WMQAX;

namespace PEX.Connectors.MQAdapter
{
    public interface IMqMessageBuilder
    {
        MQMessage Build(MqMessage message);
    }
}