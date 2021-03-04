﻿using System;
using System.Collections.Generic;
using System.Globalization;
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

            request.PaymentInstructions.ForEach(pi =>
                payloads.Add(new ProcessTrailPayload()
                {
                    Id = Id + "-1",
                    Payload = new EnvelopPayload()
                    {
                        Payment = null,
                        Reason = new Reason("error", "Error in astrea response"),
                        Original = new Original(request.Mt.ToString()),
                        Act = new Act("passThrough", "passThrough"),
                        Assess = new Assess()
                        {
                            Hints = new List<Hint>()
                            {
                                new Hint("status", new List<string>(){"INVOKE_ASTREA_HTTP", "ASTREA:RESPONSE"})
                            },
                            RiskLevel = 0
                        }
                    }
                })
            ); ;

            return payloads;
        }

        protected General SetupGeneral(AssessmentRequest assessment)
        {
            var formattedTime = DateTime.ParseExact(assessment.Mt103Model.ApplicationHeader.OutputDate + assessment.Mt103Model.ApplicationHeader.OutputTime,
                "yyMMddHHmm", CultureInfo.InvariantCulture);
            return new General
            {
                Time = formattedTime,
                Bo = GetBo(assessment.Mt103Model.UserHeader.UniqueEndToEndTransactionReference, assessment.Mt103Model.SenderToReceiverInformation),
                Event = new Event(AstreaClientConstants.EventType_Offered, $"{assessment.BasketIdentity}|ERROR")
            };

        }
    }
}
