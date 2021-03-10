using System;
using System.Collections.Generic;
using XB.Astrea.Client.Constants;
using XB.Astrea.Client.Messages.Assessment;

namespace XB.Astrea.Client.Messages.ProcessTrail
{

    public record Event(string Type, string Id);

    public record Location(string Type = "sys", string Id = "SWIFT");

    public record Context(string Cli, string Env, string Sch);

    public record References(string Reference, string Type);

    public record ProcessTrailActor(string Id, string Type, List<string> Roles);

    public record ProcessTrailRemittanceInfo(string Info, string Type);

    public record Tag(string K, string V);

    public record Original(string Content, string ContentType = "swift/plain", string Encoding = "none");

    public record Reason(string Code, string Text);

    public record Act(string RecommendedAction, string ExecutedAction);

    public record Ref(string Id, string Type, string IdType);

    public record Bo(string Id, string Type, string IdType = AstreaClientConstants.Tag121);

    public record Account(string Id, string Bic)
    {
        public string IdType => Id.Length >= 11 && char.IsLetter(Id[0]) && char.IsLetter(Id[1])
            ? AstreaClientConstants.Iban
            : AstreaClientConstants.Bban;
    }

    public record General
    {
        public Bo Bo { get; init; }
        public DateTime Time { get; init; }
        public IEnumerable<Ref> Refs { get; init; }
        public Event Event { get; init; }
        public Location Location { get; init; }
        public IEnumerable<ProcessTrailActor> Actors { get; init; }
        public IEnumerable<Tag> Tags { get; init; }
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
        public IEnumerable<Hint> Hints { get; init; }
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
        public IEnumerable<References> References { get; init; }
        public IEnumerable<ProcessTrailRemittanceInfo> RemittanceInfos { get; init; }
    }
}
