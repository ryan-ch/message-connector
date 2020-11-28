using System.Collections.Generic;
using Newtonsoft.Json;

namespace XB.Astrea.Client.Messages.Assessment
{
    public class Response
    {
        public string RequestIdentity { get; set; }
        public string Identity { get; set; }
        public string RiskLevel { get; set; }
        public List<Hint> Hints { get; set; }
        public string AssessmentStatus { get; set; }
        public List<string> RiskyInstructions { get; set; }
        public List<Result> Results { get; set; }
    }

    public class Extras
    {
        public string PhysicalNonPhysical { get; set; }
        public string OrderingCustomerAddress { get; set; }
        public string OrderingCustomerAccount { get; set; }
        public string OrderingCustomerName { get; set; }
        public string FullName { get; set; }
        public object OrderingBankBIC { get; set; }
        public string AccountHolderID { get; set; }
        public string SoleProprietorship { get; set; }
        public string AccountNumber { get; set; }
    }

    public class Result
    {
        public string OrderIdentity { get; set; }
        public string RiskLevel { get; set; }
        public List<Hint> Hints { get; set; }
        public Extras Extras { get; set; }
    }
}
