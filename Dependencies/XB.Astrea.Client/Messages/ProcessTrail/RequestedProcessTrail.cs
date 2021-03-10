using System.Collections.Generic;
using System.Linq;
using XB.Astrea.Client.Constants;
using XB.Astrea.Client.Messages.Assessment;

namespace XB.Astrea.Client.Messages.ProcessTrail
{
    public class RequestedProcessTrail : ProcessTrailBase
    {
        public RequestedProcessTrail(AssessmentRequest request, string appVersion) : base(appVersion)
        {
            General = SetupGeneral(request);
            Payloads = SetupPayloads(request);
        }

        private List<ProcessTrailPayload> SetupPayloads(AssessmentRequest request)
        {
            var payloads = new List<ProcessTrailPayload>();

            request.PaymentInstructions.ForEach(pi =>
                payloads.Add(new ProcessTrailPayload
                {
                    Id = Id + "-1",
                    Payload = new EnvelopPayload
                    {
                        Payment = new Payment
                        {
                            InstructedDate = pi.InstructedDate.ToString("yyyy-MM-dd"),
                            InstructedAmount = pi.Amount,
                            InstructedCurrency = pi.Currency,
                            References = GetReferences(request.BasketIdentity, request.Mt103Model.SenderReference),
                            RemittanceInfos = GetRemittanceInfos(request.Mt103Model),
                            DebitAccount = new List<Account> { new Account(pi.DebitAccount.First().Identity, "") },
                            //TODO: What IdTypes are there?
                            CreditAccount = new List<Account> { new Account(pi.CreditAccount.First().Identity, "") }
                        },
                        Original = new Original(request.Mt)
                    }
                })
            );

            return payloads;
        }

        private General SetupGeneral(AssessmentRequest request)
        {
            return new General
            {
                Time = request.Mt103Model.ApplicationHeader.OutputDate,
                Bo = GetBo(request.Mt103Model.UserHeader.UniqueEndToEndTransactionReference, request.Mt103Model.SenderToReceiverInformation),
                Event = new Event(AstreaClientConstants.EventType_Requested, $"{request.BasketIdentity}|{request.Mt103Model.ApplicationHeader.OutputDate.ToString(AstreaClientConstants.SwedishUtcDateFormat)}")
            };
        }
    }
}
