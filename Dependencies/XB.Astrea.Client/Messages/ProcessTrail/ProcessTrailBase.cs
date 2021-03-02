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
                int.TryParse(assessmentResult.RiskLevel, out int riskLevel);
                payloads.Add(new ProcessTrailPayload
                {
                    Id = Id + "-1",
                    Payload = new EnvelopPayload
                    {
                        Act = new Act(actAction, actAction),
                        Reason = reason,

                        Payment = new Payment
                        {
                            References = new List<References>
                            {
                                new References(assessmentResult.OrderIdentity, "swift.tag121.uniqueId"),
                                new References(parsedMt.SenderReference, "swift.tag20.sendersRef")
                            },
                            RemittanceInfos = string.IsNullOrEmpty(parsedMt.SenderToReceiverInformation) && string.IsNullOrEmpty(parsedMt.RemittanceInformation) ?
                                              null : new List<ProcessTrailRemittanceInfo>
                                              {
                                                  !string.IsNullOrEmpty(parsedMt.SenderToReceiverInformation) ? new ProcessTrailRemittanceInfo(parsedMt.SenderToReceiverInformation, "swift.tag72.senderToReceiver") : null,
                                                  !string.IsNullOrEmpty(parsedMt.RemittanceInformation) ? new ProcessTrailRemittanceInfo(parsedMt.RemittanceInformation, "swift.tag70.remittanceInfo") : null
                                              }
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
            var formattedTime = DateTime.ParseExact(parsedMt.ApplicationHeader.OutputDate + parsedMt.ApplicationHeader.OutputTime,
                "yyMMddHHmm", CultureInfo.InvariantCulture);
            return new General
            {
                Time = formattedTime,
                Bo = new Bo
                {
                    Id = response.Identity,
                    IdType = "swift.tag121",
                    Type = GetBoType(parsedMt)
                },
                Refs = new List<Ref> { new Ref(response.Identity, AstreaClientConstants.ProcessTrailRefType, AstreaClientConstants.ProcessTrailRefIdType) },
                Event = new Event(eventType, $"{parsedMt.UserHeader.UniqueEndToEndTransactionReference}|{formattedTime.ToString(AstreaClientConstants.DateFormat)}")
            };
        }

        protected string GetBoType(Mt103Message model)
        {
            return model.SenderToReceiverInformation != null &&
                   model.SenderToReceiverInformation.StartsWith("/DOM") ?
                "seb.payments.se.incoming.domestic" : "seb.payments.se.incoming.xb";
        }
    }

    public record Event(string Type, string Id);

    public record Location(string Type = "sys", string Id = "SWIFT");

    public record Context(string Cli, string Env, string Sch);

    public record Account(string Id, string IdType, string Bic);

    public record References(string Reference, string Type);

    public record ProcessTrailActor(string Id, string Type, List<string> Roles);

    public record ProcessTrailRemittanceInfo(string Info, string Type);

    public record Tag(string K, string V);

    public record Original(string Content, string ContentType = "swift/plain", string Encoding = "none");

    public record Reason(string Code, string Text);

    public record Act(string RecommendedAction, string ExecutedAction);

    public record Ref(string Id, string Type, string IdType);


    public record General
    {
        public Bo Bo { get; init; }
        public DateTime Time { get; init; }
        public List<Ref> Refs { get; init; }
        public Event Event { get; init; }
        public Location Location { get; init; }
        public List<ProcessTrailActor> Actors { get; init; }
        public List<Tag> Tags { get; init; }
    }

    public record Bo
    {
        public string Type { get; init; } //seb.payments.se.incoming.(domestic/xb)
        public string Id { get; init; } //swift block3.tag121
        public string IdType { get; init; } = AstreaClientConstants.BoIdType;
    }

    public record ProcessTrailPayload
    {
        public string Id { get; init; }
        public string Encoding { get; init; } = AstreaClientConstants.PayloadEncoding;
        public string Store { get; init; } = AstreaClientConstants.PayloadStore;
        public string SchemaVersion { get; init; } = AstreaClientConstants.PayloadSchemaVersion;
        public EnvelopPayload Payload { get; init; }
    }

    public record EnvelopPayload
    {
        public Payment Payment { get; init; }
        public Original Original { get; init; }
        public PayloadExtras Extras { get; init; }
        public Assess Assess { get; init; }
        public Reason Reason { get; init; }
        public Act Act { get; init; }
    }

    public record Assess
    {
        public string Id { get; init; }
        public int RiskLevel { get; init; }
        // TODO: we should set Hints as init-only properties
        public IEnumerable<Hint> Hints { get; set; }
        public AssessExtras Extras { get; init; }
    }

    public record AssessExtras
    {
        public string OrderingCustomerAccount { get; init; }
        public string OrderingCustomerName { get; init; }
        public string OrderingCustomerAddress { get; init; }
        public string OrderingBankBic { get; init; }
        public string BeneficiaryCustomerAccount { get; init; }
        public string BeneficiaryCustomerName { get; init; }
        public string BeneficiaryCustomerAddress { get; init; }
        public string BeneficiaryBankBic { get; init; }
        public string CustomerId { get; init; }
        public bool Physical { get; init; }
        public bool SoleProprietorship { get; init; }
        public string FullName { get; init; }
        public string AccountNumber { get; init; }
        public string RawMessage { get; init; }
    }

    public record PayloadExtras
    {
        public string SwiftBeneficiaryCustomerAccount { get; init; }
        public string SwiftBeneficiaryCustomerName { get; init; }
        public string SwiftBeneficiaryCustomerAddress { get; init; }
        public string SwiftBeneficiaryBankBIC { get; init; }
        public string SwiftRawMessage { get; init; }
    }

    public record Payment
    {
        public string InstructedDate { get; init; }
        public string ExecutionDate { get; init; }
        public decimal InstructedAmount { get; init; }
        public string InstructedCurrency { get; init; }
        public IEnumerable<Account> DebitAccount { get; init; }
        public IEnumerable<Account> CreditAccount { get; init; }
        public List<References> References { get; init; }
        public List<ProcessTrailRemittanceInfo> RemittanceInfos { get; init; }
    }
}
