using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using XB.Astrea.Client.Constants;
using XB.MtParser.Mt103;

namespace XB.Astrea.Client.Messages.Assessment
{
    public class AssessmentRequest
    {
        public AssessmentRequest(Mt103Message mt103)
        {
            OrderIdentity = Guid.NewGuid().ToString();
            BasketIdentity = mt103.UserHeader.UniqueEndToEndTransactionReference;
            PaymentInstructions = SetupPaymentInstruction(mt103);
            TargetState = AstreaClientConstants.EventType_Requested;
            Tags = new Tags();
            Mt103Model = mt103;
            Mt = mt103.OriginalSwiftMessage;
        }

        public string OrderIdentity { get; set; }
        public string BasketIdentity { get; set; }
        public List<PaymentInstruction> PaymentInstructions { get; set; } = new List<PaymentInstruction>();
        [JsonProperty(Required = Required.Always)]
        public string TargetState { get; set; }
        public Tags Tags { get; set; }
        [JsonIgnore]
        public string Mt { get; set; }
        //TODO: As there are many fields that are unconfirmed, we will use this for data transfer for fields that have not been classified
        [JsonIgnore]
        public Mt103Message Mt103Model { get; set; }

        private static List<PaymentInstruction> SetupPaymentInstruction(Mt103Message mt)
        {
            var paymentInstructionList = new List<PaymentInstruction>
            {
                new PaymentInstruction()
                {
                    Identity = mt.UserHeader.UniqueEndToEndTransactionReference,
                    PaymentType = "seb.payment.se.swift." + mt.BankOperationCode,
                    RegistrationTime = DateTime.Now,
                    InstructedDate = mt.ValueDate,
                    Amount = mt.SettledAmount,
                    Currency = mt.Currency,
                    DebitAccount = new List<Account>() { GetDebitAccount(mt) },
                    CreditAccount = new List<Account> { GetCreditAccount(mt) },
                    RemittanceInfo = new List<RemittanceInfo>(),
                    InstructionContext = new InstructionContext(new List<string>(),"", "0"),
                }
            };
            return paymentInstructionList;
        }

        private static Account GetDebitAccount(Mt103Message model)
        {
            var account = string.Empty;

            if (!string.IsNullOrWhiteSpace(model.OrderingCustomer.Account))
                account = model.OrderingCustomer.Account;

            return new Account(account);
        }

        private static Account GetCreditAccount(Mt103Message model)
        {
            var account = string.Empty;

            if (!string.IsNullOrEmpty(model.BeneficiaryCustomer.Account))
                account = model.BeneficiaryCustomer.Account;

            return new Account(account);
        }
    }

    public record RegisteringParty(string AuthId, string SebId);
    public record RemittanceInfo(string Info, string Type);
    public record InstructionContext(List<string> Debtors, string Beneficiary, string DebitAccountAvailableAmount);
    public record Actor(string SebId, string AuthId);
    public record Principal(string SebId, string AuthId);
    public record Tags();

    public record Account
    {
        public Account(string account)
        {
            Identity = account;
            Type = account.Length >= 11 && char.IsLetter(account[0]) && char.IsLetter(account[1])
                ? AstreaClientConstants.Iban
                : AstreaClientConstants.Bban;
        }

        public string Identity { get; init; }
        public string Type { get; init; }
    }

    public record PaymentInstruction
    {
        public string Identity { get; init; }
        //TODO: Check if PaymentType is "se.seb.payment.foreign.swift." + Mt103.{4:->:23B:
        public string PaymentType { get; init; }
        [JsonIgnore]
        public RegisteringParty RegisteringParty { get; init; }
        public DateTime RegistrationTime { get; init; }
        public DateTime InstructedDate { get; init; }
        public decimal Amount { get; init; }
        public string Currency { get; init; }
        public List<Account> DebitAccount { get; init; }
        public List<Account> CreditAccount { get; init; }
        public List<RemittanceInfo> RemittanceInfo { get; init; }
        public InstructionContext InstructionContext { get; init; }
    }
}
