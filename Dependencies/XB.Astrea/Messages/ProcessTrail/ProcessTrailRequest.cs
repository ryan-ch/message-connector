using System;
using System.Collections.Generic;
using System.Linq;
using XB.Astrea.Client.Messages.Assessment;

namespace XB.Astrea.Client.Messages.ProcessTrail
{
    public class ProcessTrailRequest : ProcessTrailBase
    {
        public ProcessTrailRequest(AssessmentRequest request) : base(request) { }

        protected override List<ProcessTrailPayload> SetupPayloads(AssessmentRequest request)
        {
            var payloads = new List<ProcessTrailPayload>();

            request.PaymentInstructions.ForEach(pi =>
                payloads.Add(new ProcessTrailPayload()
                {
                    Id = Id + "-1",
                    //TODO: Make dynamic so we can change this or automatically detect this
                    Encoding = "plain/json",
                    //TODO: Make dynamic so we can change this
                    Store = "ses-fcp-payment-orders",
                    Payload = new EnvelopPayload()
                    {
                        Payment = new Payment()
                        {
                            InstructedDate = pi.InstructedDate,
                            InstructedAmount = pi.Amount,
                            InstructedCurrency = pi.Currency,
                            References = new List<References>()
                            {

                            },
                            DebitAccount = new List<Account>()
                            {
                                new Account(pi.DebitAccount.First().Identity,Constants.Iban)
                            },
                            CreditAccount = new List<Account>()
                            {
                                //TODO: What types are there?
                                new Account(pi.CreditAccount.First().Identity,Constants.Iban)
                            }
                        }
                    }
                })
            );

            return payloads;
        }

        protected override General SetupGeneral(AssessmentRequest assessment)
        {
            return new General
            {
                Time = Time,
                Event = new Event(Constants.EventType_Requested, Guid.NewGuid()),
                Bo = new Bo
                {
                    Id = Id,
                    Type = "seb.payments.se.incoming.xb",
                    IdType = "swift.block3.tag121"
                },
                Refs = new List<Ref> {
                    new Ref
                    {
                        Id = assessment.PaymentInstructions.First().Identity,
                        Type = "bo",
                        IdType = "ses.fcp.payment.order.swift"
                    }
                }
            };
        }

        #region Redundant methods
        protected override List<ProcessTrailPayload> SetupPayloads(AssessmentResponse response)
        {
            throw new NotImplementedException();
        }
        protected override General SetupGeneral(AssessmentResponse response)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
