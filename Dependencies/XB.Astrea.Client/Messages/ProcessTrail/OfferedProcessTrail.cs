using System.Collections.Generic;
using System.Linq;
using XB.Astrea.Client.Constants;
using XB.Astrea.Client.Messages.Assessment;
using XB.MtParser.Mt103;

namespace XB.Astrea.Client.Messages.ProcessTrail
{
    public class OfferedProcessTrail : ProcessTrailBase
    {
        public OfferedProcessTrail(AssessmentResponse response, string appVersion, Mt103Message parsedMt, bool timeout = false) : base(appVersion)
        {

            General = SetupGeneral(AstreaClientConstants.EventType_Offered, response, parsedMt);
            Payloads = SetupPayloads(response, parsedMt, GetReason(timeout), AstreaClientConstants.Action_PassThrough);

            if (!timeout) return;

            var payload = Payloads.First().Payload;
            payload.Assess.Hints = payload.Assess.Hints.Append(new Hint("timeout", new List<string>() { "HUBERT_TIMEOUT" }));
        }

        private static Reason GetReason(bool timeout)
        {
            return timeout ? new Reason("timeout", "Timeout approval decision received in response") : null;
        }
    }
}
