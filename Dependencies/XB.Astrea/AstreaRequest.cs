using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace XB.Astrea
{
    public class AstreaRequest
    {
        public Guid BasketIdentity { get; set; } = Guid.NewGuid();
        public List<PaymentInstruction> PaymentInstructions { get; set; }
        public Actor Actor { get; set; }
        public Principal Principal { get; set; }
        [JsonProperty(Required = Required.Always)]
        public string TargetState { get; set; }
        public Tags Tags { get; set; }

        [JsonIgnore]
        public string Mt { get; set; }
    }

    public class RegisteringParty
    {
        public string AuthId { get; set; }
        public string SebId { get; set; }
    }

    public class DebitAccount
    {
        public string Type { get; set; }
        public string BankIdentity { get; set; }
        public string Identity { get; set; }
    }

    public class CreditAccount
    {
        public string Type { get; set; }
        public string BankIdentity { get; set; }
        public string Identity { get; set; }
    }

    public class RemittanceInfo
    {
        public string Info { get; set; }
        public string Type { get; set; }
    }

    public class InstructionContext
    {
        public List<string> Debtors { get; set; }
        public double DebitAccountAvailableAmount { get; set; }
        public string DebitAccountCurrency { get; set; }
    }

    public class PaymentInstruction
    {
        public string Identity { get; set; }
        public string PaymentType { get; set; }
        public RegisteringParty RegisteringParty { get; set; }
        public DateTime RegistrationTime { get; set; }
        public DateTime InstructedDate { get; set; }
        public int Amount { get; set; }
        public string Currency { get; set; }
        public List<DebitAccount> DebitAccount { get; set; }
        public List<CreditAccount> CreditAccount { get; set; }
        public List<RemittanceInfo> RemittanceInfo { get; set; }
        public InstructionContext InstructionContext { get; set; }
    }

    public class Actor
    {
        public string SebId { get; set; }
        public string AuthId { get; set; }
    }

    public class Principal
    {
        public string AuthId { get; set; }
        public string SebId { get; set; }
    }

    public class Tags
    {
    }
}
