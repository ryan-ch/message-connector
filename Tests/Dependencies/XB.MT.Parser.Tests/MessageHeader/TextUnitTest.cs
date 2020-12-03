using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using XB.MT.Parser.Model.Common;
using XB.MT.Parser.Model.Text.MT103.Fields;
using Xunit;

namespace MT103XUnitTestProject.MessageHeader
{
    class TextUnitTest
    {

        internal static void ValidateField13C(Field13C field13C, string timeIndication)
        {
            Assert.NotNull(field13C);
            ValidateCommonFieldDelimiters(field13C.CommonFieldDelimiters, "13C");
            Assert.Equal(timeIndication, field13C.TimeIndication);
        }

        internal static void ValidateField20(Field20 field20, string senderReference)
        {
            Assert.NotNull(field20);
            ValidateCommonFieldDelimiters(field20.CommonFieldDelimiters, "20");
            Assert.Equal(senderReference, field20.SenderReference);
        }

        internal static void ValidateField23B(Field23B field23B, string bankOperationCode)
        {
            Assert.NotNull(field23B);
            ValidateCommonFieldDelimiters(field23B.CommonFieldDelimiters, "23B");
            Assert.Equal(bankOperationCode, field23B.BankOperationCode);
        }

        internal static void ValidateField23E(Field23E field23E, string instructionCode)
        {
            Assert.NotNull(field23E);
            ValidateCommonFieldDelimiters(field23E.CommonFieldDelimiters, "23E");
            Assert.Equal(instructionCode, field23E.InstructionCode);
        }

        internal static void ValidateField32A(Field32A field32A, string valueDate_Currency_InterbankSettledAmount, string valueDate,
                                                         string currency, string interbankSettledAmount)
        {
            Assert.NotNull(field32A);
            ValidateCommonFieldDelimiters(field32A.CommonFieldDelimiters, "32A");
            Assert.Equal(valueDate_Currency_InterbankSettledAmount, field32A.ValueDate_Currency_InterbankSettledAmount);

            Assert.Equal(DateTime.ParseExact(valueDate, "yyMMdd", CultureInfo.InvariantCulture), field32A.ValueDate);
            Assert.Equal(valueDate, field32A.ValueDate.ToString("yyMMdd", CultureInfo.InvariantCulture));

            Assert.Equal(currency, field32A.Currency);

            // Compare as string
            Assert.Equal(interbankSettledAmount, GetDoubleFormatedToString(interbankSettledAmount, field32A.InterbankSettledAmount));
            // Compare as double
            Assert.Equal(double.Parse(interbankSettledAmount), field32A.InterbankSettledAmount);
        }

        internal static void ValidateField33B(Field33B field33B, string currency_InstructedAmount, string currency, string instructedAmount)
        {
            Assert.NotNull(field33B);
            ValidateCommonFieldDelimiters(field33B.CommonFieldDelimiters, "33B");
            Assert.Equal(currency_InstructedAmount, field33B.Currency_InstructedAmount);
            Assert.Equal(currency, field33B.Currency);

            // Compare as string
            Assert.Equal(instructedAmount, GetDoubleFormatedToString(instructedAmount, field33B.InstructedAmount));
            // Compare as double
            Assert.Equal(double.Parse(instructedAmount), field33B.InstructedAmount);
        }

        internal static void ValidateField50K(Field50K field50K, string orderingCustomer, 
                                              AccountCrLF accountCrLF, List<NameOrAddressCrLf> nameAndAdressList)
        {
            Assert.NotNull(field50K);
            ValidateCommonFieldDelimiters(field50K.CommonFieldDelimiters, "50K");
            Assert.Equal(orderingCustomer, field50K.OrderingCustomer);
            Assert.True(accountCrLF.Equals(field50K.AccountCrLf));
            //Assert.True(field50K.NameAndAddressList.Equals(nameAndAdressList)); // Doesn't work, call ValidateNameAndAddressCrLfList instead
            ValidateNameAndAddressCrLfList(nameAndAdressList, field50K.NameAndAddressList);
        }

        private static void ValidateNameAndAddressCrLfList(List<NameOrAddressCrLf> facit, List<NameOrAddressCrLf> parsed)
        {
            Assert.Equal(facit.Count, parsed.Count);
            for (int i = 0; i < facit.Count; i++)
            {
                Assert.True(facit[i].Equals(parsed[i]));
            }
        }

        internal static void ValidateField52A(Field52A field52A, string orderingInstitution)
        {
            Assert.NotNull(field52A);
            ValidateCommonFieldDelimiters(field52A.CommonFieldDelimiters, "52A");
            Assert.Equal(orderingInstitution, field52A.OrderingInstitution);
        }

        internal static void ValidateField59(Field59 field59, string beneficiaryCustomer)
        {
            Assert.NotNull(field59);
            ValidateCommonFieldDelimiters(field59.CommonFieldDelimiters, "59");
            Assert.Equal(beneficiaryCustomer, field59.BeneficiaryCustomer);
        }

        internal static void ValidateField70(Field70 field70, string remittanceInformation)
        {
            Assert.NotNull(field70);
            ValidateCommonFieldDelimiters(field70.CommonFieldDelimiters, "70");
            Assert.Equal(remittanceInformation, field70.RemittanceInformation);
        }

        internal static void ValidateField71A(Field71A field71A, string detailsOfCharges)
        {
            Assert.NotNull(field71A);
            ValidateCommonFieldDelimiters(field71A.CommonFieldDelimiters, "71A");
            Assert.Equal(detailsOfCharges, field71A.DetailsOfCharges);
        }

        internal static void ValidateField71F(Field71F field71F, string sendersCharges)
        {
            Assert.NotNull(field71F);
            ValidateCommonFieldDelimiters(field71F.CommonFieldDelimiters, "71F");
            Assert.Equal(sendersCharges, field71F.SendersCharges);
        }

        private static void ValidateCommonFieldDelimiters(CommonFieldDelimiters commonFieldDelimiters, string fieldIdentifier)
        {
            Assert.Equal(":", commonFieldDelimiters.StartOfFieldDelimiter);
            Assert.Equal(fieldIdentifier, commonFieldDelimiters.FieldIdentifier);
            Assert.Equal(":", commonFieldDelimiters.Separator);
            Assert.Equal("\r", commonFieldDelimiters.CarriageReturn);
            Assert.Equal("\n", commonFieldDelimiters.LineFeed);
        }

        private static string GetDoubleFormatedToString(string amountPatternToFollow, double amountToFormat)
        {
            int lenght = amountPatternToFollow.Length;
            int ix = amountPatternToFollow.IndexOf(",");
            string format = "";
            if (ix < 0)
            {
                format = "0";
            }
            else
            {
                format = "0.";
                for (int i = 0; i < (lenght - ix - 1); ix++)
                {
                    format += "0";
                }
            }
            return amountToFormat.ToString(format) + (format.Equals("0.") ? "," : "");
        }


    }
}
