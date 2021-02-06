using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using XB.Astrea.Client.Constants;
using XB.MT.Parser.Model;
using XB.MT.Parser.Model.Common;
using XB.MT.Parser.Model.Text.MT103;


namespace XB.Astrea.Client.Messages.Assessment
{
    public class AssessmentRequest
    {
        public AssessmentRequest(MT103SingleCustomerCreditTransferModel mt103)
        {
            OrderIdentity = Guid.NewGuid().ToString();
            BasketIdentity = mt103.UserHeader.Tag121_UniqueEndToEndTransactionReference.UniqueEndToEndTransactionReference;
            PaymentInstructions = SetupPaymentInstruction(mt103);
            TargetState = AstreaClientConstants.EventType_Requested;
            Tags = new Tags();
            Mt103Model = mt103;
            Mt = mt103.OriginalSWIFTmessage;
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
        public MT103SingleCustomerCreditTransferModel Mt103Model { get; set; }

        private static List<PaymentInstruction> SetupPaymentInstruction(MT103SingleCustomerCreditTransferModel mt)
        {
            var paymentInstructionList = new List<PaymentInstruction>
            {
                new PaymentInstruction()
                {
                    Identity = mt.UserHeader.Tag121_UniqueEndToEndTransactionReference.UniqueEndToEndTransactionReference,
                    PaymentType = "seb.payment.se.swift." + mt.MT103SingleCustomerCreditTransferBlockText.Field23B.BankOperationCode,
                    RegistrationTime = DateTime.Now,
                    InstructedDate = mt.MT103SingleCustomerCreditTransferBlockText.Field32A.ValueDate,
                    Amount = mt.MT103SingleCustomerCreditTransferBlockText.Field32A.InterbankSettledAmount,
                    Currency = mt.MT103SingleCustomerCreditTransferBlockText.Field32A.Currency,
                    DebitAccount = new List<Account>() { GetDebitAccount(mt) },
                    CreditAccount = new List<Account> { GetCreditAccount(mt) },
                    RemittanceInfo = new List<RemittanceInfo>(),
                    InstructionContext = new InstructionContext(new List<string>(),"", "0"),
                }
            };
            return paymentInstructionList;
        }

        private static Account GetDebitAccount(MT103SingleCustomerCreditTransferModel model)
        {
            var account = string.Empty;

            if (model.MT103SingleCustomerCreditTransferBlockText.Field50K != null)
            {
                account = model.MT103SingleCustomerCreditTransferBlockText.Field50K.AccountCrLf.Account;
            } else if (model.MT103SingleCustomerCreditTransferBlockText.Field50F != null)
            {
                account = model.MT103SingleCustomerCreditTransferBlockText.Field50F.PartyIdentifier.AccountCrLf.Account;
            } else if (model.MT103SingleCustomerCreditTransferBlockText.Field50A.IdentifierCodes.Count > 0)
            {
                account = model.MT103SingleCustomerCreditTransferBlockText.Field50A.AccountCrLf.Account;
            }

            return new Account(GetAccountType(account), account);
        }

        private static Account GetCreditAccount(MT103SingleCustomerCreditTransferModel model)
        {
            var account = string.Empty;

            if (model.MT103SingleCustomerCreditTransferBlockText.Field59 != null)
            {
                account = model.MT103SingleCustomerCreditTransferBlockText.Field59.AccountCrLf.Account;
            } else if (model.MT103SingleCustomerCreditTransferBlockText.Field59F != null)
            {
                account = model.MT103SingleCustomerCreditTransferBlockText.Field59F.AccountCrLf.Account;
            } else if (model.MT103SingleCustomerCreditTransferBlockText.Field59A.AdditionalRows.Count > 0)
            {
                account = model.MT103SingleCustomerCreditTransferBlockText.Field59A.AccountCrLf.Account;
            }

            return new Account(GetAccountType(account), account);
        }

        private static string GetAccountType(string account)
        {
            if (account.Length >= 11 &&
                char.IsLetter(account[0]) &&
                char.IsLetter(account[1]))
            {
                return AstreaClientConstants.Iban;
            }

            return AstreaClientConstants.Bban;
        }
    }

    public record RegisteringParty(string AuthId, string SebId);
    public record Account(string Type, string Identity);
    public record RemittanceInfo(string Info, string Type);
    public record InstructionContext(List<string> Debtors, string Beneficiary, string DebitAccountAvailableAmount);
    public record Actor(string SebId, string AuthId);
    public record Principal(string SebId, string AuthId);
    public record Tags();

    public class PaymentInstruction
    {
        public string Identity { get; set; }
        //TODO: Check if PaymentType is "se.seb.payment.foreign.swift." + Mt103.{4:->:23B:
        public string PaymentType { get; set; }
        [JsonIgnore]
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
