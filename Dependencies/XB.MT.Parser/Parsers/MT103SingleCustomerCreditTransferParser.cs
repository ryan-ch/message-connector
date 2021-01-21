using System;
using XB.MT.Common;
using XB.MT.Common.Model.Common;
using XB.MT.Parser.Model;
using XB.MT.Parser.Model.Common;
using XB.MT.Parser.Model.Text.MT103;
using XB.MT.Parser.Model.Text.MT103.Fields;

namespace XB.MT.Parser.Parsers
{
    public class MT103SingleCustomerCreditTransferParser : MT103Parser
    {
        // TODO: Change to this version of ParseMessage
        //public MT103SingleCustomerCreditTransferModel ParseMessage(string message, MT103Version version)
        public MT103SingleCustomerCreditTransferModel ParseMessage(string message)
        {
            // TODO: string message, MT103Version version should maybe be set via a constructor now when the class isn't static
            MT103SingleCustomerCreditTransferModel mT103Instans = null;

            MT103Version version = MT103Version.V_2020; //TODO: Remove this row
            if (version != MT103Version.V_2020)
            {
                UnhandledVersion(version);
            }
            else
            {
                mT103Instans = new MT103SingleCustomerCreditTransferModel();
                mT103Instans.SetOriginalSWIFTmessageIfNull(message);
                PopulateHeaders(mT103Instans, message, version);
                PopulateText(mT103Instans, message, version);

            }

            return mT103Instans;
        }


        private void PopulateText(MT103SingleCustomerCreditTransferModel mT103Instans, string message, MT103Version version)
        {
            mT103Instans.SetOriginalSWIFTmessageIfNull(message);
            // Prepared to handle different versions in different ways
            if (version == MT103Version.V_2020)
            {
                int textStart = message.IndexOf("{4:");
                if (textStart >= 0)
                {
                    mT103Instans.MT103SingleCustomerCreditTransferBlockText = new MT103SingleCustomerCreditTransferText();
                    MT103SingleCustomerCreditTransferText text = mT103Instans.MT103SingleCustomerCreditTransferBlockText;
                    CommonBlockDelimiters commonBlockDelimiters = text.CommonBlockDelimiters;
                    commonBlockDelimiters.SetDefaultStartOfBlockValues("4");

                    string textMessage = "";
                    int textEnd = message.IndexOf("-}", textStart);
                    if (textEnd > -1)
                    {
                        textEnd++;
                        text.SetDefaultEndOfBlockHypen();
                        commonBlockDelimiters.SetDefaultValue(CommonBlockDelimiter.EndOfBlockDelimiter);
                        textMessage = message.Substring(textStart, textEnd - textStart + 1);
                    }
                    else
                    {
                        // Will not be set if there is no end EndOfBlockIndicator for the UserHeader?
                        textMessage = message;
                    }

                    string[] fieldIdentifiers = new string[] { "13C", "20", "23B", "23E", "32A", "33B", "50K",
                                                           "52A", "52D", "59", "70", "71A", "71F" };
                    foreach (var fieldIdentifier in fieldIdentifiers)
                    {
                        HandleField(fieldIdentifier, textMessage, text);
                    }
                }
            }
            else
            {
                UnhandledVersion(version);
            }
        }

        private static void CheckPreCrLf(MT103SingleCustomerCreditTransferText text)
        {
            if (text.PreFieldCarriageReturn == null || text.PreFieldLineFeed == null)
            {
                text.SetDefaultPreFieldCrLf();
            }
        }
        private static void HandleField(string fieldIdentifier, string textMessage, MT103SingleCustomerCreditTransferText text)
        {
            int fieldStartIx = textMessage.IndexOf(":" + fieldIdentifier + ":");
            if (fieldStartIx > -1)
            {
                CommonFieldDelimiters commonFieldDelimiters = new CommonFieldDelimiters(fieldIdentifier);
                int fieldEndIx1 = textMessage.IndexOf(Constants.CrLf + ":", fieldStartIx + fieldIdentifier.Length + 2);  // Start of next field
                int fieldEndIx2 = textMessage.IndexOf(Constants.CrLf + "-}", fieldStartIx + fieldIdentifier.Length + 2); // End of block Text
                if (fieldEndIx1 < 0 && fieldEndIx2 < 0)
                {
                    throw new Exception("End of field ('" + Constants.CrLf + ":') and end of block ('" + Constants.CrLf + "-}') not found in textMessage after index " +
                                        (fieldStartIx + fieldIdentifier.Length + 2) + ", textMessage: '" + textMessage + "'.");
                }

                int fieldEndIx = GetSmallestValidIx(fieldEndIx1, fieldEndIx2);
                if (fieldEndIx > -1)
                {
                    commonFieldDelimiters.SetCarriageReturnNewLine();
                    string fieldValue = textMessage.Substring(fieldStartIx + fieldIdentifier.Length + 2,
                                                              fieldEndIx - (fieldStartIx + fieldIdentifier.Length + 2));
                    CreateField(fieldIdentifier, text, commonFieldDelimiters, fieldValue);
                }
            }
        }

        private static int GetSmallestValidIx(int ix1, int ix2)
        {
            int smallestIx = -1;
            if (ix1 > -1 && ix2 > -1)
            {
                smallestIx = ix1 < ix2 ? ix1 : ix2;
            }
            else if (ix1 < 0 && ix2 > -1)
            {
                smallestIx = ix2;
            }
            else if (ix1 > -1 && ix2 < 0)
            {
                smallestIx = ix1;
            }
            return smallestIx;
        }

        private static void CreateField(string fieldIdentifier, MT103SingleCustomerCreditTransferText text, CommonFieldDelimiters commonFieldDelimiters, string fieldValue)
        {
            switch (fieldIdentifier)
            {
                case "13C":
                    text.Field13C = new Field13C(commonFieldDelimiters, fieldValue);
                    break;
                case "20":
                    text.Field20 = new Field20(commonFieldDelimiters, fieldValue);
                    break;
                case "23B":
                    text.Field23B = new Field23B(commonFieldDelimiters, fieldValue);
                    break;
                case "23E":
                    text.Field23E = new Field23E(commonFieldDelimiters, fieldValue);
                    break;
                case "32A":
                    text.Field32A = new Field32A(commonFieldDelimiters, fieldValue);
                    break;
                case "33B":
                    text.Field33B = new Field33B(commonFieldDelimiters, fieldValue);
                    break;
                case "50K":
                    text.Field50K = new Field50K(commonFieldDelimiters, fieldValue);
                    break;
                case "52A":
                    text.Field52A = new Field52A(commonFieldDelimiters, fieldValue);
                    break;
                case "52D":
                    text.Field52D = new Field52D(commonFieldDelimiters, fieldValue);
                    break;
                case "59":
                    text.Field59 = new Field59(commonFieldDelimiters, fieldValue);
                    break;
                case "70":
                    text.Field70 = new Field70(commonFieldDelimiters, fieldValue);
                    break;
                case "71A":
                    text.Field71A = new Field71A(commonFieldDelimiters, fieldValue);
                    break;
                case "71F":
                    text.Field71F = new Field71F(commonFieldDelimiters, fieldValue);
                    break;
                default:
                    throw new Exception("Program error, fieldIdentifier has an invalid value: '" + fieldIdentifier + ".");
            }
        }
    }
}
