using System;
using System.Collections.Generic;
using System.Linq;
using XB.Astrea.Client.Constants;
using XB.Astrea.Client.Messages.Assessment;
using XB.MtParser.Mt103;

namespace XB.Astrea.Client.Messages.ProcessTrail
{
    public abstract class ProcessTrailBase
    {
        protected ProcessTrailBase(string appVersion)
        {
            Id = Guid.NewGuid();
            Time = DateTime.Now;
            System = AstreaClientConstants.System;
            Context = SetupContext(appVersion);
        }

        public Guid Id { get; set; }
        public DateTime Time { get; set; }
        public string System { get; set; }
        public Context Context { get; set; }
        public General General { get; set; }
        public List<ProcessTrailPayload> Payloads { get; set; }

        private Context SetupContext(string version) =>
            new Context($"{System}-v{version}", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"), AstreaClientConstants.ProcessTrailSchemaVersion);

        protected List<ProcessTrailPayload> SetupPayloads(AssessmentResponse response, Mt103Message parsedMt, string actAction, string hubertStatus)
        {
            var payloads = new List<ProcessTrailPayload>();
            var reason = GetReason(hubertStatus);

            response.Results.ForEach(assessmentResult =>
            {
                _ = int.TryParse(assessmentResult.RiskLevel, out int riskLevel);
                var hints = assessmentResult.Hints?.Where(h => h.Name != null);
                if (hubertStatus == AstreaClientConstants.Hubert_Timeout)
                    hints = hints.Concat(new[] {new Hint("timeout", new[] {"HUBERT_TIMEOUT"})});

                payloads.Add(new ProcessTrailPayload
                {
                    Id = Id + "-1",
                    Payload = new EnvelopPayload
                    {
                        Act = new Act(actAction, actAction),
                        Reason = reason,

                        Payment = new Payment
                        {
                            References = GetReferences(assessmentResult.OrderIdentity, parsedMt.SenderReference),
                            RemittanceInfos = GetRemittanceInfos(parsedMt),
                        },
                        Extras = new PayloadExtras(),
                        Assess = new Assess
                        {
                            Id = assessmentResult.OrderIdentity,
                            RiskLevel = riskLevel,
                            Hints = hints,
                            Extras = new AssessExtras
                            {
                                BeneficiaryCustomerAccount = assessmentResult.Extras.AccountNumber,
                                BeneficiaryCustomerName = assessmentResult.Extras.FullName,
                                OrderingCustomerAccount = assessmentResult.Extras.OrderingCustomerAccount
                            }
                        },
                        Original = riskLevel > 0 ? new Original(parsedMt.OriginalSwiftMessage) : null
                    }
                });
            });

            return payloads;
        }

        protected General SetupGeneral(string eventType, AssessmentResponse response, Mt103Message parsedMt)
        {
            return new General
            {
                Time = parsedMt.ApplicationHeader.OutputDate,
                Bo = GetBo(response.Identity, parsedMt.SenderToReceiverInformation),
                Refs = new[] { new Ref(response.Identity, AstreaClientConstants.ProcessTrailRefType, AstreaClientConstants.ProcessTrailRefIdType) },
                Event = new Event(eventType, $"{parsedMt.UserHeader.UniqueEndToEndTransactionReference}|{parsedMt.ApplicationHeader.OutputDate.ToString(AstreaClientConstants.SwedishUtcDateFormat)}")
            };
        }

        protected Bo GetBo(string identity, string senderToReceiverInformation)
        {
            var type = !string.IsNullOrWhiteSpace(senderToReceiverInformation) && senderToReceiverInformation.StartsWith("/DOM")
                ? AstreaClientConstants.IncomingDomestic
                : AstreaClientConstants.IncomingXb;

            return new Bo(identity, type);
        }

        protected IEnumerable<References> GetReferences(string identity, string senderReference) =>
            new[]
            {
                new References(identity, AstreaClientConstants.Tag121Id),
                new References(senderReference, AstreaClientConstants.Tag20SenderRef)
            };

        protected IEnumerable<ProcessTrailRemittanceInfo> GetRemittanceInfos(Mt103Message mt103)
        {
            var remittanceInfos = new List<ProcessTrailRemittanceInfo>();

            if (!string.IsNullOrWhiteSpace(mt103.SenderToReceiverInformation))
                remittanceInfos.Add(new ProcessTrailRemittanceInfo(mt103.SenderToReceiverInformation, AstreaClientConstants.Tag72SenderToReceiver));
            if (!string.IsNullOrWhiteSpace(mt103.RemittanceInformation))
                remittanceInfos.Add(new ProcessTrailRemittanceInfo(mt103.RemittanceInformation, AstreaClientConstants.Tag70RemittanceInfo));

            return remittanceInfos;
        }
        
        private Reason GetReason(string hubertStatus)
        {
            return hubertStatus switch
            {
                AstreaClientConstants.Hubert_Rejected => new Reason("fcp-access", "HUB_MT103_REJECTED"),
                AstreaClientConstants.Hubert_Timeout => new Reason("timeout", "Timeout approval decision received in response"),
                _ => null
            };
        }
    }
}
