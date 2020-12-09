using System;
using System.Collections.Generic;
using XB.Astrea.Client.Messages.Assessment;

namespace XB.Astrea.Client.Messages.ProcessTrail
{
    public class RequestedProcessTrail : ProcessTrailBase
    {
        public RequestedProcessTrail(AssessmentRequest request) : base(request)
        { }

        protected override General SetupGeneral(AssessmentRequest request)
        {
            throw new NotImplementedException();
        }

        protected override General SetupGeneral(AssessmentResponse response)
        {
            throw new NotImplementedException();
        }

        protected override List<ProcessTrailPayload> SetupPayloads(AssessmentRequest request)
        {
            throw new NotImplementedException();
        }

        protected override List<ProcessTrailPayload> SetupPayloads(AssessmentResponse response)
        {
            throw new NotImplementedException();
        }
    }
}
