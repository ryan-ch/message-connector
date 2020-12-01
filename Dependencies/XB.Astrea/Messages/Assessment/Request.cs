using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace XB.Astrea.Client.Messages.Assessment
{
    public class Request
    {
        public string OrderIdentity { get; set; }
        public string BasketIdentity { get; set; }
        public List<PaymentInstruction> PaymentInstructions { get; set; } = new List<PaymentInstruction>();
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

    public class Account
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
        //TODO: Check if PaymentType is "se.seb.payment.foreign.swift." + Mt103.{4:->:23B:
        public string PaymentType { get; set; }
        public RegisteringParty RegisteringParty { get; set; }
        public DateTime RegistrationTime { get; set; }
        public DateTime InstructedDate { get; set; }
        public double Amount { get; set; }
        public string Currency { get; set; }
        public List<Account> DebitAccount { get; set; }
        public List<Account> CreditAccount { get; set; }
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
