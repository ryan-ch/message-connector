using System;
using System.Collections.Generic;
using System.Linq;

namespace XB.Astrea.Client.ProcessTrail
{
    public static class RequestHelper
    {
        public static Request ParseAstreaRequestToAstreaProcessTrailRequest(Astrea.Client.Assessment.Request request, string type)
        {
            //TODO: Look over how the ids are set and they are referring to correct ids
            var id = Guid.NewGuid();
            var timeNow = DateTime.Now;

            var processTrailRequest = new Request()
            {
                Id = id,
                Time = timeNow,
                //TODO: System: Get this from configuration or something else! Need to be flexible if another system will use this lib
                System = "Hubert",
                Context = SetupContext(),
                General = SetupGeneral(type, timeNow, id),
                Payloads = SetupPayloads(request)
            };

            return processTrailRequest;
        }

        private static List<Payload> SetupPayloads(Astrea.Client.Assessment.Request request)
        {
            var payloads = new List<Payload>();

            request.PaymentInstructions.ForEach(pi =>
                payloads.Add(new Payload()
                {
                    //TODO: Make dynamic so we can change this or automatically detect this
                    Encoding = "plain/json",
                    //TODO: Make dynamic so we can change this
                    Store = "ses-fcp-payment-orders",
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
                            new Account()
                            {
                                Id = pi.DebitAccount.First().Identity,
                                //TODO: What types are there?
                                IdType = "iban"
                            }
                        },
                        CreditAccount = new List<Account>()
                        {
                            new Account()
                            {
                                Id = pi.CreditAccount.First().Identity,
                                //TODO: What types are there?
                                IdType = "iban"
                            }
                        }
                    }
                })
            );

            return payloads;
        }

        private static General SetupGeneral(string type, DateTime timeNow, Guid id)
        {
            return new General()
            {
                Time = timeNow,
                Event = new Event()
                {
                    Id = id,
                    Type = type
                },
                Bo = new Bo()
                {
                    Id = id,
                    Type = "other",
                    IdType = "ses.fcp.payment.order.swift"
                },
                Refs = new List<Ref>() {
                    new Ref()
                    {
                        Id = id,
                        Type = "bo",
                        IdType = "ses.fcp.payment.order.swift"
                    }
                }
            };
        }

        private static Context SetupContext()
        {
            return new Context()
            {
                //TODO: Cli: is this the application id of the system that generates the process trail?
                Cli = "Astrea Connector 1.0",
                Env = "tst"
            };
        }
    }
}
