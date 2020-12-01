using System;
using System.Collections.Generic;
using System.Linq;
using XB.Astrea.Client.Messages.Assessment;

namespace XB.Astrea.Client.Messages.ProcessTrail
{
    public static class Factory
    {
        public static Requested GetRequestedProcessTrail(Astrea.Client.Messages.Assessment.Request assessment)
        {
            //TODO: Look over how the ids are set and they are referring to correct ids
            var processTrailId = Guid.NewGuid();
            var timeNow = DateTime.Now;

            var processTrailRequest = new Requested()
            {
                Id = processTrailId,
                Time = timeNow,
                //TODO: System: Get this from configuration or something else! Need to be flexible if another system will use this lib
                System = "Hubert",
                Context = SetupContext(),
                General = SetupGeneral("requested", timeNow, processTrailId, assessment),
                Payloads = SetupPayloadsForRequested(assessment, processTrailId)
            };

            return processTrailRequest;
        }

        public static Offered GetOfferedProcessTrail(Response assessment)
        {
            //TODO: Look over how the ids are set and they are referring to correct ids
            var id = Guid.NewGuid();
            var timeNow = DateTime.Now;

            var processTrailRequest = new Offered()
            {
                Id = id,
                Time = timeNow,
                //TODO: System: Get this from configuration or something else! Need to be flexible if another system will use this lib
                System = "Hubert",
                Context = SetupContext(),
                General = SetupGeneralForOffered("offered", timeNow, id, assessment),
                Payloads = SetupPayloadsForOffered(assessment, id)
            };

            return processTrailRequest;
        }

        public static Rejected GetRejectedProcessTrail(Astrea.Client.Messages.Assessment.Request assessment)
        {
            throw new NotImplementedException();
        }

        private static List<Payloads> SetupPayloadsForOffered(Response assessment, Guid eventId)
        {
            var payloads = new List<Payloads>();

            assessment.Results.ForEach(pi =>
                payloads.Add(new Payloads()
                {
                    Id = eventId.ToString() + "-1",
                    //TODO: Make dynamic so we can change this or automatically detect this
                    Encoding = "plain/json",
                    //TODO: Make dynamic so we can change this
                    Store = "ses-fcp-payment-orders",
                    Payload = new EnvelopPayload()
                    {
                        Extras = new Extras()
                        {
                        }
                    }
                })
            );

            return payloads;
        }

        private static List<Payloads> SetupPayloadsForRequested(Astrea.Client.Messages.Assessment.Request request, Guid eventId)
        {
            var payloads = new List<Payloads>();

            request.PaymentInstructions.ForEach(pi =>
                payloads.Add(new Payloads()
                {
                    Id = eventId + "-1",
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
                    }
                })
            );

            return payloads;
        }

        private static General SetupGeneral(string type, DateTime timeNow, Guid id, Astrea.Client.Messages.Assessment.Request assessment)
        {
            return new General()
            {
                Time = timeNow,
                Event = new Event()
                {
                    Id = Guid.NewGuid(),
                    Type = type
                },
                Bo = new Bo()
                {
                    Id = id,
                    Type = "seb.payments.se.incoming.xb",
                    IdType = "swift.block3.tag121"
                },
                Refs = new List<Ref>() {
                    new Ref()
                    {
                        Id = assessment.PaymentInstructions.First().Identity,
                        Type = "bo",
                        IdType = "ses.fcp.payment.order.swift"
                    }
                }
            };
        }

        private static General SetupGeneralForOffered(string type, DateTime timeNow, Guid id, Astrea.Client.Messages.Assessment.Response assessment)
        {
            return new General()
            {
                Time = timeNow,
                Event = new Event()
                {
                    //TODO: Find out what Id should be mapped to here, or is it a new guid?
                    Id = Guid.NewGuid(),
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
                        Id = assessment.Results.First().OrderIdentity,
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
                //TODO: Cli: is this the application eventId of the system that generates the process trail?
                Cli = "Astrea Connector 1.0",
                Env = "tst"
            };
        }
    }
}
