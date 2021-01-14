﻿using System;
using System.Collections.Generic;
using XB.Astrea.Client.Constants;
using XB.Astrea.Client.Messages.Assessment;
using XB.MT.Parser.Model;

namespace XB.Astrea.Client.Messages.ProcessTrail
{
    /**
     * TODOs:
     * MT Parsing for the following tags: 72, 52, 57
    **/

    public abstract class ProcessTrailBase
    {
        private readonly string version;

        private ProcessTrailBase(string appVersion)
        {
            version = appVersion;
            Id = Guid.NewGuid();
            Time = DateTime.Now;
            System = AstreaClientConstants.System;
            Context = SetupContext();
        }

        protected ProcessTrailBase(AssessmentRequest assessmentRequest, string appVersion) : this(appVersion)
        {
            General = SetupGeneral(assessmentRequest);
            Payloads = SetupPayloads(assessmentRequest);
        }

        protected ProcessTrailBase(AssessmentResponse assessmentResponse, string appVersion, MT103SingleCustomerCreditTransferModel parsedMt) : this(appVersion)
        {
            General = SetupGeneral(assessmentResponse, parsedMt);
            Payloads = SetupPayloads(assessmentResponse, parsedMt);
        }

        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime Time { get; set; } = DateTime.Now;
        public string System { get; set; }
        public Context Context { get; set; }
        public General General { get; set; }
        public List<ProcessTrailPayload> Payloads { get; set; }

        protected Context SetupContext()
        {
            //TODO: Cli: is this the application eventId of the system that generates the process trail?
            return new Context($"{System}-v{version}", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"), AstreaClientConstants.ProcessTrailSchemaVersion);
        }

        protected string GetBOType(AssessmentRequest assessmentRequest)
        {
            //TODO: check tag72 - return "seb.payments.se.incoming.domestic" or "seb.payments.se.incoming.xb"
            return "seb.payments.se.incoming.domestic";
        }

        protected string GetBOType(AssessmentResponse assessmentResponse)
        {
            //TODO: check tag72 - return "seb.payments.se.incoming.domestic" or "seb.payments.se.incoming.xb"
            return "seb.payments.se.incoming.domestic";
        }

        protected abstract List<ProcessTrailPayload> SetupPayloads(AssessmentRequest request);

        protected virtual List<ProcessTrailPayload> SetupPayloads(AssessmentResponse response, MT103SingleCustomerCreditTransferModel parsedMt) {
            
            var payloads = new List<ProcessTrailPayload>();
            response.Results.ForEach(pi => { 
                int.TryParse(pi.RiskLevel, out int riskLevel);
                payloads.Add(new ProcessTrailPayload()
                {
                    Id = Id + "-1",
                    Payload = new EnvelopPayload()
                    {
                        Payment = new Payment()
                        {
                            References = new List<References>()
                            {
                                new References(pi.OrderIdentity, "swift.tag121.uniqueId"),
                                new References(parsedMt.MT103SingleCustomerCreditTransferBlockText.Field20.SenderReference, "swift.tag20.sendersRef"),
                                new References(parsedMt.MT103SingleCustomerCreditTransferBlockText.Field70.RemittanceInformation, "swift.tag20.remittanceInfo")
                            },
                        },
                        Extras = new PayloadExtras()
                        {
                        },
                        Assess = new Assess()
                        {
                            Id = pi.OrderIdentity,
                            RiskLevel = riskLevel,
                            Hints = pi.Hints,
                            Extras = new AssessExtras()
                            {
                                BeneficiaryCustomerAccount = pi.Extras.AccountNumber,
                                BeneficiaryCustomerName = pi.Extras.FullName
                            }
                        },
                        Original = riskLevel > 0 ? new Original(parsedMt.ToString()) : null
                    }
                });
            });

            return payloads;
        }

        protected virtual General SetupGeneral(AssessmentRequest request) {
            return new General
            {
                Time = Time,
                Bo =
                {
                    Id = request.BasketIdentity,
                    IdType = "swift.tag121",
                    Type = GetBOType(request)
                },
                Actors = new List<ProcessTrailActor>()
                {
                    new ProcessTrailActor(request.MtModel.ApplicationHeaderInputMessage.DestinationAddress, AstreaClientConstants.ActorIdType, new List<string>() { AstreaClientConstants.ActorRole })
                },
                Location = new Location()
            };
        }

        protected virtual General SetupGeneral(AssessmentResponse response, MT103SingleCustomerCreditTransferModel parsedMt) {
            return new General
            {
                Time = Time,
                Bo =
                {
                    Id = response.RequestIdentity,
                    IdType = "swift.tag121",
                    Type = GetBOType(response)
                },
                Refs = new List<Ref> {
                    new Ref
                    {
                        Id = response.Identity,
                        IdType = AstreaClientConstants.ProcessTrailRefIdType,
                        Type = AstreaClientConstants.ProcessTrailRefType
                    }
                },
                Location = new Location(),
                Actors = new List<ProcessTrailActor>()
                {
                    new ProcessTrailActor(parsedMt.ApplicationHeaderInputMessage.DestinationAddress, AstreaClientConstants.ActorIdType, new List<string>() { AstreaClientConstants.ActorRole })
                }
            };
        }
    }

    //TODO: GUID or string? => Verify excel requirements: Should we use only tag121 from swift or use tag121|general.time as id
    public record Event(string Type, string Id);

    public record Location(string Type = "sys", string Id = "SWIFT");

    public record Context(string Cli, string Env, string Sch);

    public record Account(string Id, string IdType, string bic);

    public record References(string Type, string Reference);

    public record ProcessTrailActor(string Id, string Type, List<string> Roles);

    public record ProcessTrailRemittanceInfo(string Info, string Type);

    public record Tag(string K, string V);

    public record Original(string Content, string ContentType = "swift/plain", string Encoding = "none");

    public record Reason(string code, string text);

    public record Act(string recommendedAction, string executedAction);

    public class General
    {
        public Bo Bo { get; set; }
        public DateTime Time { get; set; }
        public List<Ref> Refs { get; set; }
        public Event Event { get; set; }
        public Location Location { get; set; }
        public List<ProcessTrailActor> Actors { get; set; }
        public List<Tag> Tags { get; set; }
    }

    public class Bo
    {
        public string Type { get; set; } //seb.payments.se.incoming.(domestic/xb)
        public string Id { get; set; } //swift block3.tag121
        public string IdType { get; set; } = AstreaClientConstants.BoIdType;
    }

    public class Ref
    {
        public string Type { get; set; } = AstreaClientConstants.ProcessTrailRefType;
        public string Id { get; set; }
        public string IdType { get; set; } = AstreaClientConstants.ProcessTrailRefIdType;
    }

    public class ProcessTrailPayload
    {
        public string Id { get; set; }
        public string Encoding { get; set; } = AstreaClientConstants.PayloadEncoding;
        public string Store { get; set; } = AstreaClientConstants.PayloadStore;
        public string SchemaVersion { get; set; } = AstreaClientConstants.PayloadSchemaVersion;
        public EnvelopPayload Payload { get; set; }
    }

    public class EnvelopPayload
    {
        public Payment Payment { get; set; }
        public Original Original { get; set; }
        public PayloadExtras Extras { get; set; }
        public Assess Assess { get; set; }
        public Reason Reason { get; set; }
        public Act Act { get; set; }
    }

    public class Assess
    {
        public string Id { get; set; }
        public int RiskLevel { get; set; }
        public List<Hint> Hints { get; set; }
        public AssessExtras Extras { get; set; }
    }

    public class AssessExtras
    {
        public string OrderingCustomerAccount { get; set; }
        public string OrderingCustomerName { get; set; }
        public string OrderingCustomerAddress { get; set; }
        public string OrderingBankBic { get; set; }
        public string BeneficiaryCustomerAccount { get; set; }
        public string BeneficiaryCustomerName { get; set; }
        public string BeneficiaryCustomerAddress { get; set; }
        public string BeneficiaryBankBic { get; set; }
        public string CustomerId { get; set; }
        public bool Physical { get; set; }
        public bool SoleProprietorship { get; set; }
        public string FullName { get; set; }
        public string AccountNumber { get; set; }
        public string RawMessage { get; set; }
    }

    public class PayloadExtras
    {
        public string SwiftBeneficiaryCustomerAccount { get; set; }
        public string SwiftBeneficiaryCustomerName { get; set; }
        public string SwiftBeneficiaryCustomerAddress { get; set; }
        public string SwiftBeneficiaryBankBIC { get; set; }
        public string SwiftRawMessage { get; set; }
    }

    public class Payment
    {
        public string InstructedDate { get; set; }
        public string ExecutionDate { get; set; }
        public double InstructedAmount { get; set; }
        public string InstructedCurrency { get; set; }
        public List<Account> DebitAccount { get; set; }
        public List<Account> CreditAccount { get; set; }
        public List<References> References { get; set; }
        public List<ProcessTrailRemittanceInfo> RemittanceInfos { get; set; }
    }
}
