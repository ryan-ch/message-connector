using System;
using System.Collections.Generic;
using System.Linq;
using XB.Astrea.Client.Constants;
using XB.Astrea.Client.Messages.Assessment;
using XB.MT.Parser.Model;

namespace XB.Astrea.Client.Messages.ProcessTrail
{
    public class OfferedProcessTrail : ProcessTrailBase
    {
        public OfferedProcessTrail(AssessmentResponse response, string appVersion, MT103SingleCustomerCreditTransferModel parsedMt) : base(response, appVersion, parsedMt) { }

        protected override General SetupGeneral(AssessmentResponse response, MT103SingleCustomerCreditTransferModel parsedMt)
        {
            var general = base.SetupGeneral(response, parsedMt);
            general.Event = new Event(AstreaClientConstants.EventType_Offered, $"{parsedMt.UserHeader.Tag121_UniqueEndToEndTransactionReference.UniqueEndToEndTransactionReference}|{general.Time.ToString(AstreaClientConstants.DateFormat)}");
            return general;
        }
        protected override List<ProcessTrailPayload> SetupPayloads(AssessmentResponse response, MT103SingleCustomerCreditTransferModel parsedMt)
        {
            var payloads = base.SetupPayloads(response, parsedMt);

            var index = 0;
            payloads.ForEach(pl =>
            {
                pl.Payload.Act = new Act(AstreaClientConstants.Action_PassThrough, AstreaClientConstants.Action_PassThrough);
                pl.Payload.Assess.Extras.OrderingCustomerAccount =
                    response.Results[index].Extras.OrderingCustomerAccount;
                index++;
            });

            return payloads;
        }

        #region Redundant methods
        protected override General SetupGeneral(AssessmentRequest request)
        {
            throw new NotImplementedException();
        }
        protected override List<ProcessTrailPayload> SetupPayloads(AssessmentRequest request)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
