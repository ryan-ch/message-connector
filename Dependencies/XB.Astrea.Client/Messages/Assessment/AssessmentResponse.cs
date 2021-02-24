using System.Collections.Generic;

namespace XB.Astrea.Client.Messages.Assessment
{
    public record AssessmentResponse
    {
        public string RequestIdentity { get; init; }
        public string Identity { get; init; }
        public string RiskLevel { get; init; }
        public List<Hint> Hints { get; init; }
        public string AssessmentStatus { get; init; }
        public List<string> RiskyInstructions { get; init; }
        public List<AssessmentResult> Results { get; init; }
    }

    public record ExtraAssessmentInfo
    {
        public string PhysicalNonPhysical { get; init; }
        public string OrderingCustomerAddress { get; init; }
        public string OrderingCustomerAccount { get; init; }
        public string OrderingCustomerName { get; init; }
        public string FullName { get; init; }
        public object OrderingBankBIC { get; init; }
        public string AccountHolderID { get; init; }
        public string SoleProprietorship { get; init; }
        public string AccountNumber { get; init; }
    }

    public record AssessmentResult
    {
        public string OrderIdentity { get; init; }
        public string RiskLevel { get; init; }
        public List<Hint> Hints { get; init; }
        public ExtraAssessmentInfo Extras { get; init; }
    }
}
