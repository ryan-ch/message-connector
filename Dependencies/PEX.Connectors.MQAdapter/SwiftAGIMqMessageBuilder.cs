using IBM.WMQ;

namespace PEX.Connectors.MQAdapter
{
    public class SwiftAGIMqMessageBuilder : MqMessageBuilder
    {
        protected override void SetSpecifics()
        {
            MqMessage.MQMD.MsgType = MQC.MQMT_DATAGRAM;
            MqMessage.MQMD.Expiry = MQC.MQEI_UNLIMITED;
            MqMessage.MQMD.MsgId = new byte[1]; //TODO What id? Guid?
            MqMessage.MQMD.Feedback = MQC.MQFB_NONE;
            MqMessage.MQMD.Encoding = MQC.MQENC_NATIVE;
            MqMessage.MQMD.CodedCharacterSetId = 1208;
            //Message will not work with the following line uncommented:
            //MqMessage.MQMD.Format = new byte[1]; //TODO Change
            MqMessage.MQMD.Report = 0;
            MqMessage.MQMD.Persistence = MQC.MQPER_NOT_PERSISTENT;
            MqMessage.MQMD.Offset = 0;
            MqMessage.MQMD.MsgFlags = MQC.MQMF_NONE;
            MqMessage.MQMD.OriginalLength = MQC.MQOL_UNDEFINED;
        }
        protected override string Format => MQC.MQFMT_NONE;
    }
}
