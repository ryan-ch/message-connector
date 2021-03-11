using System.Collections.Generic;
using XB.Astrea.Client.Constants;
using XB.Astrea.Client.Messages.Assessment;

namespace XB.Astrea.Client.Messages.ProcessTrail
{
    public class OfferedTimeOutProcessTrail : ProcessTrailBase
    {
        public OfferedTimeOutProcessTrail(AssessmentRequest request, string appVersion) : base(appVersion)
        {
            General = SetupGeneral(request);
            Payloads = SetupPayloads(request);
        }

        protected List<ProcessTrailPayload> SetupPayloads(AssessmentRequest request)
        {
            var payloads = new List<ProcessTrailPayload>();

            request.PaymentInstructions.ForEach(_ =>
                payloads.Add(new ProcessTrailPayload
                {
                    Id = Id + "-1",
                    Payload = new EnvelopPayload
                    {
                        Payment = null,
                        Reason = new Reason("error", "Error in astrea response"),
                        Original = new Original(request.Mt),
                        Act = new Act("passThrough", "passThrough"),
                        Assess = new Assess
                        {
                            Hints = new List<Hint>
                            {
                                new Hint("status", new List<string>(){"INVOKE_ASTREA_HTTP", "ASTREA:RESPONSE"})
                            },
                            RiskLevel = 0
                        }
                    }
                })
            );

            return payloads;
        }

        protected General SetupGeneral(AssessmentRequest assessment)
        {
            return new General
            {
                Time = assessment.Mt103Model.ApplicationHeader.OutputDate,
                Bo = GetBo(assessment.Mt103Model.UserHeader.UniqueEndToEndTransactionReference, assessment.Mt103Model.SenderToReceiverInformation),
                Event = new Event(AstreaClientConstants.EventType_Offered, $"{assessment.BasketIdentity}|ERROR")
            };
        }
    }
}
