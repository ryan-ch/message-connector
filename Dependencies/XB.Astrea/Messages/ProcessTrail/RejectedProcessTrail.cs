using XB.Astrea.Client.Constants;
using XB.Astrea.Client.Messages.Assessment;
using XB.MtParser.Mt103;

namespace XB.Astrea.Client.Messages.ProcessTrail
{
    public class RejectedProcessTrail : ProcessTrailBase
    {
        public RejectedProcessTrail(AssessmentResponse response, string appVersion, Mt103Message parsedMt) : base(appVersion)
        {
            General = SetupGeneral(AstreaClientConstants.EventType_Rejected, response, parsedMt);
            Payloads = SetupPayloads(response, parsedMt, new Reason("fcp-access", "HUB_MT103_REJECTED"), AstreaClientConstants.Action_Block);
        }
    }
}
