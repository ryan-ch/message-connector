using XB.Astrea.Client.Constants;
using XB.Astrea.Client.Messages.Assessment;
using XB.MtParser.Mt103;

namespace XB.Astrea.Client.Messages.ProcessTrail
{
    public class RejectedProcessTrail : ProcessTrailBase
    {
        public RejectedProcessTrail(AssessmentResponse response, string appVersion, Mt103Message parsedMt, string hubertStatus = AstreaClientConstants.Hubert_Rejected) : base(appVersion)
        {
            General = SetupGeneral(AstreaClientConstants.EventType_Rejected, response, parsedMt);
            Payloads = SetupPayloads(response, parsedMt, AstreaClientConstants.Action_Block, hubertStatus);
        }
    }
}
