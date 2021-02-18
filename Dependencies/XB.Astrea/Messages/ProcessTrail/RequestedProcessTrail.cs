using System;
using System.Collections.Generic;
using System.Globalization;
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
                            References = new List<References>
                            {
                                new References(request.BasketIdentity, "swift.tag121.uniqueId"),
                                new References(request.Mt103Model.SenderReference, "swift.tag20.sendersRef"),
                            },
                            RemittanceInfos = string.IsNullOrEmpty(request.Mt103Model.SenderToReceiverInformation) && string.IsNullOrEmpty(request.Mt103Model.RemittanceInformation) ?
                                              null : new List<ProcessTrailRemittanceInfo>
                                              {
                                                  !string.IsNullOrEmpty(request.Mt103Model.SenderToReceiverInformation) ? new ProcessTrailRemittanceInfo(request.Mt103Model.SenderToReceiverInformation, "swift.tag72.senderToReceiver") : null,
                                                  !string.IsNullOrEmpty(request.Mt103Model.RemittanceInformation) ? new ProcessTrailRemittanceInfo(request.Mt103Model.RemittanceInformation, "swift.tag70.remittanceInfo") : null
                                              },
                            DebitAccount = new List<Account>
                            {
                                new Account(pi.DebitAccount.First().Identity, "other", "")
                            },
                            CreditAccount = new List<Account>
                            {
                                //TODO: What types are there?
                                new Account(pi.CreditAccount.First().Identity,"other", "")
                            }
                        },
                        Original = new Original(request.Mt)
                    }
                })
            );

            return payloads;
        }

        private General SetupGeneral(AssessmentRequest request)
        {
            var formattedTime = DateTime.ParseExact(request.Mt103Model.ApplicationHeader.OutputDate + request.Mt103Model.ApplicationHeader.OutputTime,
                "yyMMddHHmm", CultureInfo.InvariantCulture);
            return new General
            {
                Time = formattedTime,
                Bo = new Bo
                {
                    Id = request.Mt103Model.UserHeader.UniqueEndToEndTransactionReference,
                    IdType = "swift.tag121",
                    Type = GetBoType(request.Mt103Model)
                },
                Event = new Event(AstreaClientConstants.EventType_Requested, $"{request.BasketIdentity}|{formattedTime.ToString(AstreaClientConstants.DateFormat)}")
            };
        }
    }
}
