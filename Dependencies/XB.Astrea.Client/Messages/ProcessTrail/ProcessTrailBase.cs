using System;
using System.Collections.Generic;
using System.Globalization;
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

        protected List<ProcessTrailPayload> SetupPayloads(AssessmentResponse response, Mt103Message parsedMt, Reason reason, string actAction)
        {
            var payloads = new List<ProcessTrailPayload>();

            response.Results.ForEach(assessmentResult =>
            {
                _ = int.TryParse(assessmentResult.RiskLevel, out int riskLevel);
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
                            Hints = assessmentResult.Hints,
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
            var formattedTime = DateTime.ParseExact(parsedMt.ApplicationHeader.OutputDate + parsedMt.ApplicationHeader.OutputTime, "yyMMddHHmm", CultureInfo.InvariantCulture);
            return new General
            {
                Time = formattedTime,
                Bo = GetBo(response.Identity, parsedMt.SenderToReceiverInformation),
                Refs = new List<Ref> { new Ref(response.Identity, AstreaClientConstants.ProcessTrailRefType, AstreaClientConstants.ProcessTrailRefIdType) },
                Event = new Event(eventType, $"{parsedMt.UserHeader.UniqueEndToEndTransactionReference}|{formattedTime.ToString(AstreaClientConstants.DateFormat)}")
            };
        }

        protected Bo GetBo(string identity, string senderToReceiverInformation)
        {
            var type = !string.IsNullOrWhiteSpace(senderToReceiverInformation) && senderToReceiverInformation.StartsWith("/DOM")
                ? AstreaClientConstants.IncomingDomestic
                : AstreaClientConstants.IncomingXb;

            return new Bo(identity, type);
        }

        protected IEnumerable<References> GetReferences(string identity, string senderReference)
        {
            return new[]
            {
                new References(identity, AstreaClientConstants.Tag121Id),
                new References(senderReference, AstreaClientConstants.Tag20SenderRef)
            };
        }

        protected IEnumerable<ProcessTrailRemittanceInfo> GetRemittanceInfos(Mt103Message mt103)
        {
            if (string.IsNullOrWhiteSpace(mt103.SenderToReceiverInformation) && string.IsNullOrWhiteSpace(mt103.RemittanceInformation))
                return null;

            return new List<ProcessTrailRemittanceInfo>
            {
                string.IsNullOrWhiteSpace(mt103.SenderToReceiverInformation)
                    ? null
                    : new ProcessTrailRemittanceInfo(mt103.SenderToReceiverInformation, AstreaClientConstants.Tag72SenderToReceiver),
                string.IsNullOrWhiteSpace(mt103.RemittanceInformation)
                    ? null
                    : new ProcessTrailRemittanceInfo(mt103.RemittanceInformation, AstreaClientConstants.Tag70RemittanceInfo)
            };
        }
    }
}
