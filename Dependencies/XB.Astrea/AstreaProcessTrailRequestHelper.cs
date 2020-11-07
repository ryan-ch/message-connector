using System;
using System.Collections.Generic;
using System.Text;

namespace XB.Astrea.Client
{
    public static class AstreaProcessTrailRequestHelper
    {
        public static AstreaProcessTrailRequest ParseAstreaRequestToAstreaProcessTrailRequest(AstreaRequest request)
        {
            var boId = Guid.NewGuid();

            var processTrailRequest = new AstreaProcessTrailRequest();
            processTrailRequest.General = new General()
            {
                Bo = new Bo()
                {
                    Id = boId
                },
                Refs = new Ref[] {
                    new Ref() { Id = boId }
                }
            };

            return processTrailRequest;
        }
    }
}
