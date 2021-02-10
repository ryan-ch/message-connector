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

        protected override General SetupGeneral(AssessmentResponse response, MT103SingleCustomerCreditTransferModel parsedMt)
        {
            var general = base.SetupGeneral(response, parsedMt);
            general.Event = new Event(AstreaClientConstants.EventType_Rejected, $"{parsedMt.UserHeader.Tag121_UniqueEndToEndTransactionReference.UniqueEndToEndTransactionReference}|{general.Time.ToString(AstreaClientConstants.DateFormat)}");
            return general;
        }

        protected override List<ProcessTrailPayload> SetupPayloads(AssessmentRequest request)
        {
            throw new NotImplementedException();
        }

        protected override List<ProcessTrailPayload> SetupPayloads(AssessmentResponse response, MT103SingleCustomerCreditTransferModel parsedMt)
        {
            var payloads = base.SetupPayloads(response, parsedMt);

            var index = 0;
            payloads.ForEach(pl =>
            {
                pl.Payload.Reason = new Reason("fcp-access", "HUB_MT103_REJECTED");
                pl.Payload.Act = new Act(AstreaClientConstants.Action_Block, AstreaClientConstants.Action_Block);
                pl.Payload.Assess.Extras.OrderingCustomerAccount =
                    response.Results[index].Extras.OrderingCustomerAccount;
                index++;
            });

            return payloads;
        }
    }
}
