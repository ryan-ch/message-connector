using System;
using System.Collections.Generic;
using System.Linq;
using XB.Astrea.Client.Messages.Assessment;

namespace XB.Astrea.Client.Messages.ProcessTrail
{
    public class OfferedProcessTrail : ProcessTrailBase
    {
        public OfferedProcessTrail(AssessmentResponse response) : base(response)
        { }

        protected override General SetupGeneral(AssessmentResponse assessment)
        {
            return new General
            {
                Time = Time,
                //TODO: Find out what Id should be mapped to here, or is it a new guid?
                Event = new Event(Constants.EventType_Offered, Guid.NewGuid()),
                Bo = new Bo
                {
                    Id = Id,
                    Type = "other",
                    IdType = "ses.fcp.payment.order.swift"
                },
                Refs = new List<Ref> {
                    new Ref
                    {
                        Id = assessment.Results.First().OrderIdentity,
                        Type = "bo",
                        IdType = "ses.fcp.payment.order.swift"
                    }
                }
            };
        }
        protected override List<ProcessTrailPayload> SetupPayloads(AssessmentResponse assessment)
        {
            var payloads = new List<ProcessTrailPayload>();

            assessment.Results.ForEach(pi =>
                payloads.Add(new ProcessTrailPayload()
                {
                    Id = Id + "-1",
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
