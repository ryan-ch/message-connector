using System;
using System.Collections.Generic;
using System.Linq;
using XB.Astrea.Client.Constants;
using XB.Astrea.Client.Messages.Assessment;
using XB.MT.Parser.Model;

namespace XB.Astrea.Client.Messages.ProcessTrail
{
    public class RequestedProcessTrail : ProcessTrailBase
    {
        public RequestedProcessTrail(AssessmentRequest request, string appVersion) : base(request, appVersion) { }

        protected override List<ProcessTrailPayload> SetupPayloads(AssessmentRequest request)
        {
            var payloads = new List<ProcessTrailPayload>();

            request.PaymentInstructions.ForEach(pi =>
                payloads.Add(new ProcessTrailPayload()
                {
                    Id = Id + "-1",
                    Payload = new EnvelopPayload()
                    {
                        Payment = new Payment()
                        {
                            InstructedDate = pi.InstructedDate.ToString("YYYY-MM-dd"),
                            InstructedAmount = pi.Amount,
                            InstructedCurrency = pi.Currency,
                            References = new List<References>()
                            {
                                new References(request.BasketIdentity, "swift.tag121.uniqueId"),
                                new References(request.Mt103Model.MT103SingleCustomerCreditTransferBlockText.Field20.SenderReference, "swift.tag20.sendersRef"),
                                request.Mt103Model.MT103SingleCustomerCreditTransferBlockText.Field70 != null ?
                                    new References(request.Mt103Model.MT103SingleCustomerCreditTransferBlockText.Field70.RemittanceInformation, "swift.tag20.remittanceInfo") : null
                            },
                            DebitAccount = new List<Account>()
                            {
                                new Account(pi.DebitAccount.First().Identity, "other", "")
                            },
                            CreditAccount = new List<Account>()
                            {
                                //TODO: What types are there?
                                new Account(pi.CreditAccount.First().Identity,"other", "")
                            }
                        },
                        Original = new Original(request.Mt.ToString()) //TODO: Store actual unparsed swift message
                    }
                })
            ); ;

            return payloads;
        }

        protected override General SetupGeneral(AssessmentRequest assessment)
        {
            var general = base.SetupGeneral(assessment);
            general.Event = new Event(AstreaClientConstants.EventType_Requested, $"{assessment.BasketIdentity}|{general.Time}");
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
