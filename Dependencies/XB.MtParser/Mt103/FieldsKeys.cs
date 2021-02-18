using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace XB.MtParser.Mt103
{
    internal static class FieldsKeys
    {
        internal const string SenderReference_20Key = "20";
        internal const string TimeIndication_13CKey = "13C";
        internal const string BankOperationCode_23BKey = "23B";
        internal const string InstructionCode_23EKey = "23E";
        internal const string TransactionTypeCode_26TEKey = "26T";
        internal const string Date_Currency_SettledAmount_32AKey = "32A";
        internal const string Currency_InstructedAmount_33BKey = "33B";
        internal const string ExchangeRate_36Key = "36";
        internal const string OrderingCustomer_50AKey = "50A";
        internal const string OrderingCustomer_50FKey = "50F";
        internal const string OrderingCustomer_50KKey = "50K";
        internal const string OrderingInstitution_52AKey = "52A";
        internal const string OrderingInstitution_52DKey = "52D";
        internal const string BeneficiaryCustomer_59Key = "59";
        internal const string BeneficiaryCustomer_59AKey = "59A";
        internal const string BeneficiaryCustomer_59FKey = "59F";
        internal const string RemittanceInformation_70Key = "70";
        internal const string DetailsOfCharges_71AKey = "71A";
        internal const string SenderCharges_71FKey = "71F";
        internal const string ReceiverCharges_71GKey = "71G";
        internal const string SenderToReceiverInformation_72Key = "72";

        internal static readonly ReadOnlyCollection<string> FieldsKeysCollection = new ReadOnlyCollection<string>(new List<string>
        {
            SenderReference_20Key,TimeIndication_13CKey,BankOperationCode_23BKey,InstructionCode_23EKey,TransactionTypeCode_26TEKey,
            Date_Currency_SettledAmount_32AKey,Currency_InstructedAmount_33BKey,ExchangeRate_36Key,OrderingCustomer_50AKey,OrderingCustomer_50FKey,
            OrderingCustomer_50KKey,OrderingInstitution_52AKey,OrderingInstitution_52DKey,BeneficiaryCustomer_59Key,BeneficiaryCustomer_59AKey,
            BeneficiaryCustomer_59FKey,RemittanceInformation_70Key,DetailsOfCharges_71AKey,SenderCharges_71FKey,ReceiverCharges_71GKey,
            SenderToReceiverInformation_72Key
        });
    }
}
