using System;
using System.Collections.Generic;
using XB.Astrea.Client.Constants;
using XB.Astrea.Client.Messages.Assessment;
using XB.MT.Parser.Model;

namespace XB.Astrea.Client.Messages.ProcessTrail
{
    public class OfferedProcessTrail : ProcessTrailBase
    {
        public OfferedProcessTrail(AssessmentResponse response, string appVersion, MT103SingleCustomerCreditTransferModel parsedMt) : base(response, appVersion, parsedMt) { }

        protected override General SetupGeneral(AssessmentResponse assessment, MT103SingleCustomerCreditTransferModel parsedMt)
        {
            var general = base.SetupGeneral(assessment, parsedMt);
            general.Event = new Event(AstreaClientConstants.EventType_Offered, $"{parsedMt.UserHeader.Tag121_UniqueEndToEndTransactionReference.UniqueEndToEndTransactionReference}|{general.Time}");
            return general;
        }
        protected override List<ProcessTrailPayload> SetupPayloads(AssessmentResponse assessment, MT103SingleCustomerCreditTransferModel parsedMt)
        {
            var payloads = base.SetupPayloads(assessment, parsedMt);

            payloads.ForEach(pl =>
            {
                pl.Payload.Act = new Act(AstreaClientConstants.Action_PassThrough, AstreaClientConstants.Action_PassThrough);
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
