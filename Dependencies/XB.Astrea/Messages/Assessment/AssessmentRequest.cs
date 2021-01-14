using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using XB.Astrea.Client.Constants;
using XB.MT.Parser.Model;

namespace XB.Astrea.Client.Messages.Assessment
{
    public class AssessmentRequest
    {
        public AssessmentRequest(MT103SingleCustomerCreditTransferModel mt)
        {
            OrderIdentity = Guid.NewGuid().ToString();
            BasketIdentity = mt.UserHeader.Tag121_UniqueEndToEndTransactionReference.UniqueEndToEndTransactionReference;
            PaymentInstructions = SetupPaymentInstruction(mt);
            Actor = new Actor("", "");// Todo:implement
            Principal = new Principal("", "");// Todo:implement
            TargetState = AstreaClientConstants.EventType_Requested;
            Tags = new Tags();
            MtModel = mt;
        }
        [JsonIgnore]
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
        //TODO: As there are many fields that are unconfirmed, we will use this for data transfer for fields that have not been classified
        [JsonIgnore]
        public MT103SingleCustomerCreditTransferModel MtModel { get; set; }

        private static List<PaymentInstruction> SetupPaymentInstruction(MT103SingleCustomerCreditTransferModel mt)
        {
            var paymentInstructionList = new List<PaymentInstruction>
            {
                new PaymentInstruction()
                {
                    Identity = mt.UserHeader.Tag121_UniqueEndToEndTransactionReference.UniqueEndToEndTransactionReference,
                    PaymentType = "seb.payment.se.swift." + mt.MT103SingleCustomerCreditTransferBlockText.Field23B,
                    RegistrationTime = DateTime.Now,
                    InstructedDate = mt.MT103SingleCustomerCreditTransferBlockText.Field32A.ValueDate,
                    Amount = mt.MT103SingleCustomerCreditTransferBlockText.Field32A.InterbankSettledAmount,
                    Currency = mt.MT103SingleCustomerCreditTransferBlockText.Field32A.Currency,
                    //TODO: check if length is greater then 11 and if first two are alphabetic
                    DebitAccount = new List<Account>() { new Account(AstreaClientConstants.Iban,"","SE2750000000056970162486") },
                    //TODO: check if length is greater then 11 and if first two are alphabetic
                    CreditAccount = new List<Account> { new Account(AstreaClientConstants.Iban,"","SE3550000000054910000003") },
                    RemittanceInfo = new List<RemittanceInfo>(),
                    InstructionContext = new InstructionContext(new List<string>{"","" },"",""),
                    RegisteringParty = new RegisteringParty("","")
                }
            };
            return paymentInstructionList;
        }
    }

    public record RegisteringParty(string AuthId, string SebId);
    public record Account(string Type, string BankIdentity, string Identity);
    public record RemittanceInfo(string Info, string Type);
    public record InstructionContext(List<string> Debtors, string DebitAccountAvailableAmount, string DebitAccountCurrency);
    public record Actor(string SebId, string AuthId);
    public record Principal(string SebId, string AuthId);
    public record Tags();

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
}
