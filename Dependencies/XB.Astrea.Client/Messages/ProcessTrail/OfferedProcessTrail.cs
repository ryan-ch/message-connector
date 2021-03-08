using System.Linq;
using XB.Astrea.Client.Constants;
using XB.Astrea.Client.Messages.Assessment;
using XB.MtParser.Mt103;

namespace XB.Astrea.Client.Messages.ProcessTrail
{
    public class OfferedProcessTrail : ProcessTrailBase
    {
        //Todo: refactor passed flag
        public OfferedProcessTrail(AssessmentResponse response, string appVersion, Mt103Message parsedMt, bool timeout = false) : base(appVersion)
        {
            General = SetupGeneral(AstreaClientConstants.EventType_Offered, response, parsedMt);
            Payloads = SetupPayloads(response, parsedMt, GetReason(timeout), AstreaClientConstants.Action_PassThrough);

            if (!timeout) return;

            var payload = Payloads.First().Payload;
            payload.Assess.Hints = payload.Assess.Hints.Append(new Hint("timeout", new[] { "HUBERT_TIMEOUT" }));
        }

        private static Reason GetReason(bool timeout)
        {
            //Todo: discuss if return should be based on Hubert response (accepted as well)
            return timeout ? new Reason("timeout", "Timeout approval decision received in response") : null;
        }
    }
}
