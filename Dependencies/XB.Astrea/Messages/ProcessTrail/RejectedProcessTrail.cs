using System;
using System.Collections.Generic;
using XB.Astrea.Client.Constants;
using XB.Astrea.Client.Messages.Assessment;
using XB.MT.Parser.Model;

namespace XB.Astrea.Client.Messages.ProcessTrail
{
    public class RejectedProcessTrail : ProcessTrailBase
    {
        public RejectedProcessTrail(AssessmentResponse response, string appVersion, MT103SingleCustomerCreditTransferModel parsedMt) : base(response, appVersion, parsedMt) {}

        protected override General SetupGeneral(AssessmentResponse assessment, MT103SingleCustomerCreditTransferModel parsedMt)
        {
            var general = base.SetupGeneral(assessment, parsedMt);
            general.Event = new Event(AstreaClientConstants.EventType_Rejected, $"{parsedMt.UserHeader.Tag121_UniqueEndToEndTransactionReference.UniqueEndToEndTransactionReference}|{general.Time}");
            return general;
        }

        protected override List<ProcessTrailPayload> SetupPayloads(AssessmentRequest request)
        {
            throw new NotImplementedException();
        }

        protected override List<ProcessTrailPayload> SetupPayloads(AssessmentResponse assessment, MT103SingleCustomerCreditTransferModel parsedMt)
        {
            var payloads = base.SetupPayloads(assessment, parsedMt);

            payloads.ForEach(pl =>
            {
                pl.Payload.Reason = new Reason("fcp-access", "TBD_ErrorCode");
                pl.Payload.Act = new Act(AstreaClientConstants.Action_Block, AstreaClientConstants.Action_Block);
            });

            return payloads;
        }
    }
}
