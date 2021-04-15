using XB.Astrea.Client.Constants;
using XB.Astrea.Client.Messages.Assessment;
using XB.MtParser.Mt103;

namespace XB.Astrea.Client.Messages.ProcessTrail
{
    public class OfferedProcessTrail : ProcessTrailBase
    {
        public OfferedProcessTrail(AssessmentResponse response, string appVersion, Mt103Message parsedMt, string hubertStatus = AstreaClientConstants.Hubert_Accepted) : base(appVersion)
        {
            General = SetupGeneral(AstreaClientConstants.EventType_Offered, response, parsedMt);
            Payloads = SetupPayloads(response, parsedMt, AstreaClientConstants.Action_PassThrough, hubertStatus);
        }
    }
}
