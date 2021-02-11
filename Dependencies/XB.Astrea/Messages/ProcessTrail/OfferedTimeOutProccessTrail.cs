using System;
using System.Collections.Generic;
using System.Linq;
using XB.Astrea.Client.Constants;
using XB.Astrea.Client.Messages.Assessment;
using XB.MT.Parser.Model;

namespace XB.Astrea.Client.Messages.ProcessTrail
{
    public class OfferedTimeOutProcessTrail : ProcessTrailBase
    {
        public OfferedTimeOutProcessTrail(AssessmentRequest request, string appVersion) : base(request, appVersion) { }

        protected override List<ProcessTrailPayload> SetupPayloads(AssessmentRequest request)
        {
            var payloads = new List<ProcessTrailPayload>();

            request.PaymentInstructions.ForEach(pi =>
                payloads.Add(new ProcessTrailPayload()
                {
                    Id = Id + "-1",
                    Payload = new EnvelopPayload()
                    {
                        Payment = null,
                        Reason = new Reason("error", "Error in astrea response"),
                        Original = new Original(request.Mt.ToString()),
                        Act = new Act("passThrough", "passThrough"),
                        Assess = new Assess()
                        {
                            Hints = new List<Hint>()
                            {
                                new Hint()
                                {
                                    Name = "status",
                                    Values = new List<string>()
                                    {
                                        "INVOKE_ASTREA_HTTP", "ASTREA:RESPONSE"
                                    }
                                }
                            },
                            RiskLevel = 0
                        }
                    }
                })
            ); ;

            return payloads;
        }

        protected override General SetupGeneral(AssessmentRequest assessment)
        {
            var general = base.SetupGeneral(assessment);
            general.Event = new Event(AstreaClientConstants.EventType_Offered, $"{assessment.BasketIdentity}|ERROR");
            return general;
        }

        #region Redundant methods
        protected override List<ProcessTrailPayload> SetupPayloads(AssessmentResponse response, MT103SingleCustomerCreditTransferModel parsedMt)
        {
            throw new NotImplementedException();
        }
        protected override General SetupGeneral(AssessmentResponse response, MT103SingleCustomerCreditTransferModel parsedMt)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
