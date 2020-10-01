using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace XB.Astrea.Client
{
    public class AstreaResponse
    {
        [JsonPropertyName("requestIdentity")]
        public string RequestIdentity { get; set; }

        [JsonPropertyName("identity")]
        public string Identity { get; set; }

        [JsonPropertyName("riskLevel")]
        public string RiskLevel { get; set; }

        [JsonPropertyName("hints")]
        public List<object> Hints { get; set; }

        [JsonPropertyName("assessmentStatus")]
        public string AssessmentStatus { get; set; }

        [JsonPropertyName("riskyInstructions")]
        public List<object> RiskyInstructions { get; set; }

        [JsonPropertyName("results")]
        public List<Result> Results { get; set; }
    }

    public class Hint
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("values")]
        public List<string> Values { get; set; }
    }

    public class Extras
    {
        [JsonPropertyName("Physical/NonPhysical")]
        public string PhysicalNonPhysical { get; set; }

        [JsonPropertyName("OrderingCustomerAddress")]
        public string OrderingCustomerAddress { get; set; }

        [JsonPropertyName("OrderingCustomerAccount")]
        public string OrderingCustomerAccount { get; set; }

        [JsonPropertyName("OrderingCustomerName")]
        public string OrderingCustomerName { get; set; }

        [JsonPropertyName("FullName")]
        public string FullName { get; set; }

        [JsonPropertyName("OrderingBankBIC")]
        public object OrderingBankBIC { get; set; }

        [JsonPropertyName("AccountHolderID")]
        public string AccountHolderID { get; set; }

        [JsonPropertyName("SoleProprietorship")]
        public string SoleProprietorship { get; set; }

        [JsonPropertyName("AccountNumber")]
        public string AccountNumber { get; set; }
    }

    public class Result
    {
        [JsonPropertyName("orderIdentity")]
        public string OrderIdentity { get; set; }

        [JsonPropertyName("riskLevel")]
        public string RiskLevel { get; set; }

        [JsonPropertyName("hints")]
        public List<Hint> Hints { get; set; }

        [JsonPropertyName("extras")]
        public Extras Extras { get; set; }
    }
}
