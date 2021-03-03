using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using XB.MtParser.Enums;
using XB.MtParser.Swift_Message;

namespace XB.MtParser.Mt103
{
    /// <summary>
    /// Represent A Single Customer Credit Transfer Message
    /// </summary>
    public record Mt103Message : SwiftMessage
    {
        private readonly string _textBlockContent;

        public Mt103Message(string rawSwiftMessage, ILogger<MtParser> logger) : base(rawSwiftMessage, logger)
        {
            if (SwiftMessageType != SwiftMessageTypes.SingleCustomerCreditTransfer)
            {
                Logger.LogError("Can't parse the message if type is not Mt103");
                return;
            }
            _textBlockContent = Blocks.FirstOrDefault(a => a.IdentifierAsInt == (int)SwiftMessageBlockIdentifiers.Text)?.Content;
            if (string.IsNullOrWhiteSpace(_textBlockContent))
                Logger.LogError("Can't parse Mt103 when text block is empty");
            else
                ExtractFields();
        }

        /// <summary>
        /// Field 20
        /// </summary>
        public string SenderReference { get; private set; }
        /// <summary>
        /// Field 13C
        /// </summary>
        //public string TimeIndication { get; private set; }
        /// <summary>
        /// Field 23B
        /// </summary>
        public OperationTypes BankOperationCode { get; private set; }
        /// <summary>
        /// Field 23E
        /// </summary>
        //public string InstructionCode { get; private set; }
        /// <summary>
        /// Field 26T
        /// </summary>
        //public string TransactionTypeCode { get; private set; }
        /// <summary>
        /// Part of Field 32A
        /// </summary>
        public DateTime ValueDate { get; private set; }
        /// <summary>
        /// Part of Field 32A
        /// </summary>
        public string Currency { get; private set; }
        /// <summary>
        /// Part of Field 32A
        /// </summary>
        public decimal SettledAmount { get; private set; }
        /// <summary>
        /// Field 33B
        /// </summary>
        //public string InstructedAmountWithCurrency { get; private set; }
        /// <summary>
        /// Field 36
        /// </summary>
        //public string ExchangeRate { get; private set; }
        /// <summary>
        /// Fields 50A, 50F or 50K
        /// </summary>
        public OrderingCustomer OrderingCustomer { get; private set; }
        /// <summary>
        /// Field 52A
        /// </summary>
        //public string OrderingInstitution { get; private set; }
        /// <summary>
        /// Fields 59, 59A or 59F
        /// </summary>
        public BeneficiaryCustomer BeneficiaryCustomer { get; private set; }
        /// <summary>
        /// Field 70
        /// </summary>
        public string RemittanceInformation { get; private set; }
        /// <summary>
        /// Field 71A
        /// </summary>
        //public DetailsOfChargesCodes DetailsOfCharges { get; private set; }
        /// <summary>
        /// Field 71F
        /// </summary>
        //public string SenderCharges { get; private set; }
        /// <summary>
        /// Field 71G
        /// </summary>
        //public string ReceiverCharges { get; private set; }
        /// <summary>
        /// Field 72
        /// </summary>
        public string SenderToReceiverInformation { get; private set; }

        private void ExtractFields()
        {
            SenderReference = ExtractFieldByKey(FieldsKeys.SenderReference_20Key);
            BankOperationCode = EnumUtil.ParseEnum(ExtractFieldByKey(FieldsKeys.BankOperationCode_23BKey), OperationTypes.Unknown);

            (ValueDate, Currency, SettledAmount) = ExtractDateCurrencyAmount();

            OrderingCustomer = new OrderingCustomer(ExtractFieldByKey(FieldsKeys.OrderingCustomer_50AKey),
                ExtractFieldByKey(FieldsKeys.OrderingCustomer_50FKey),
                ExtractFieldByKey(FieldsKeys.OrderingCustomer_50KKey));

            BeneficiaryCustomer = new BeneficiaryCustomer(ExtractFieldByKey(FieldsKeys.BeneficiaryCustomer_59Key),
                ExtractFieldByKey(FieldsKeys.BeneficiaryCustomer_59AKey),
                ExtractFieldByKey(FieldsKeys.BeneficiaryCustomer_59FKey));

            RemittanceInformation = ExtractFieldByKey(FieldsKeys.RemittanceInformation_70Key);

            SenderToReceiverInformation = ExtractFieldByKey(FieldsKeys.SenderToReceiverInformation_72Key);

            //TimeIndication = ExtractFieldByKey(FieldsKeys.TimeIndication_13CKey);
            //InstructionCode = ExtractFieldByKey(FieldsKeys.InstructionCode_23EKey);
            //TransactionTypeCode = ExtractFieldByKey(FieldsKeys.TransactionTypeCode_26TEKey);
            //InstructedAmountWithCurrency = ExtractFieldByKey(FieldsKeys.Currency_InstructedAmount_33BKey);
            // ExchangeRate = ExtractFieldByKey(FieldsKeys.ExchangeRate_36Key);
            //OrderingInstitution=ExtractFieldByKey(FieldsKeys.OrderingInstitution_52AKey);
            //DetailsOfCharges =ExtractFieldByKey(FieldsKeys.DetailsOfCharges_71AKey);
            //SenderCharges=ExtractFieldByKey(FieldsKeys.SenderCharges_71FKey);
            //ReceiverCharges = ExtractFieldByKey(FieldsKeys.ReceiverCharges_71GKey);
        }

        private string ExtractFieldByKey(string key)
        {
            var regex = Regex.Match(_textBlockContent, $":{key}:(.*?)(:|-$)", RegexOptions.Singleline);
            return regex.Success ? regex.Groups[1].Value.TrimEnd('\n', '\r') : string.Empty;
        }

        private (DateTime date, string currency, decimal amount) ExtractDateCurrencyAmount()
        {
            var Date_Currency_SettledAmount = ExtractFieldByKey(FieldsKeys.Date_Currency_SettledAmount_32AKey);
            if (string.IsNullOrWhiteSpace(Date_Currency_SettledAmount) || Date_Currency_SettledAmount.Length < 10)
            {
                Logger.LogError($"Invalid field {FieldsKeys.Date_Currency_SettledAmount_32AKey} with value: {Date_Currency_SettledAmount}");
                return default;
            }

            if (!DateTime.TryParseExact(Date_Currency_SettledAmount.Substring(0, 6), "yyMMdd", null, DateTimeStyles.None, out var parsedDate))
                Logger.LogError($"Couldn't extract Date from field {FieldsKeys.Date_Currency_SettledAmount_32AKey} with value: {Date_Currency_SettledAmount}");

            var currency = Date_Currency_SettledAmount.Substring(6, 3);

            // Todo: should we consider a formatter?
            if (!decimal.TryParse(Date_Currency_SettledAmount[9..].Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out var amount))
                Logger.LogError($"Couldn't extract SettledAmount from field {FieldsKeys.Date_Currency_SettledAmount_32AKey} with value: {Date_Currency_SettledAmount}");

            return (parsedDate, currency, amount);
        }

    }
}
