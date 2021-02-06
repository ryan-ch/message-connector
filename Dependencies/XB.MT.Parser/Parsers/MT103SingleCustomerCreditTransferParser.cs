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
                PopulateText(mT103Instans, version);
            }

            return mT103Instans;
        }


        private void PopulateText(MT103SingleCustomerCreditTransferModel mT103Instans, MT103Version version)
        {
            // Prepared to handle different versions in different ways
            if (version == MT103Version.V_2020)
            {
                if (TextHeader != null)
                {
                    int textStartIx = TextHeader.IndexOf("{4:");
                    if (textStartIx >= 0)
                    {
                        mT103Instans.MT103SingleCustomerCreditTransferBlockText = new MT103SingleCustomerCreditTransferText();
                        MT103SingleCustomerCreditTransferText text = mT103Instans.MT103SingleCustomerCreditTransferBlockText;
                        CommonBlockDelimiters commonBlockDelimiters = text.CommonBlockDelimiters;
                        commonBlockDelimiters.SetDefaultStartOfBlockValues("4");

                        //string textMessage = "";
                        int textEndIx = TextHeader.IndexOf("-}", textStartIx);
                        if (textEndIx > -1)
                        {
                            //textEndIx++;
                            text.SetDefaultEndOfBlockHypen();
                            commonBlockDelimiters.SetDefaultValue(CommonBlockDelimiter.EndOfBlockDelimiter);
                            //textMessage = TextHeader.Substring(textStartIx, textEndIx - textStartIx + 1);
                        }

                        string[] fieldIdentifiers = { MT103SingleCustomerCreditTransferText._13CKey,
                                                                   MT103SingleCustomerCreditTransferText._20Key,
                                                                   MT103SingleCustomerCreditTransferText._23BKey,
                                                                   MT103SingleCustomerCreditTransferText._23EKey,
                                                                   MT103SingleCustomerCreditTransferText._32AKey,
                                                                   MT103SingleCustomerCreditTransferText._33BKey,
                                                                   MT103SingleCustomerCreditTransferText._50AKey,
                                                                   MT103SingleCustomerCreditTransferText._50FKey,
                                                                   MT103SingleCustomerCreditTransferText._50KKey,
                                                                   MT103SingleCustomerCreditTransferText._52AKey,
                                                                   MT103SingleCustomerCreditTransferText._52DKey,
                                                                   MT103SingleCustomerCreditTransferText._59Key,
                                                                   MT103SingleCustomerCreditTransferText._59AKey,
                                                                   MT103SingleCustomerCreditTransferText._59FKey,
                                                                   MT103SingleCustomerCreditTransferText._70Key,
                                                                   MT103SingleCustomerCreditTransferText._71AKey,
                                                                   MT103SingleCustomerCreditTransferText._71FKey,
                                                                   MT103SingleCustomerCreditTransferText._72Key

                        };
                        foreach (var fieldIdentifier in fieldIdentifiers)
                        {
                            HandleField(fieldIdentifier, text);
                        }
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
        private void HandleField(string fieldIdentifier, MT103SingleCustomerCreditTransferText text)
        {
            int fieldStartIx = TextHeader.IndexOf(":" + fieldIdentifier + ":");
            if (fieldStartIx > -1)
            {
                CommonFieldDelimiters commonFieldDelimiters = new CommonFieldDelimiters(fieldIdentifier);
                int nextFieldIx = TextHeader.IndexOf(Constants.CrLf + ":", fieldStartIx + fieldIdentifier.Length + 2);  // Start of next field
                int endOfBlockIx = TextHeader.IndexOf(Constants.CrLf + "-}", fieldStartIx + fieldIdentifier.Length + 2); // End of block Text
                if (nextFieldIx < 0 && endOfBlockIx < 0)
                {
                    throw new Exception("End of field ('" + Constants.CrLf + ":') and end of block ('" + Constants.CrLf + "-}') not found in textMessage after index " +
                                        (fieldStartIx + fieldIdentifier.Length + 2) + ", textMessage: '" + TextHeader + "'.");
                }
                int fieldEndIx = GetSmallestValidIx(nextFieldIx, endOfBlockIx);
                if (fieldEndIx > -1)
                {
                    commonFieldDelimiters.SetCarriageReturnNewLine();
                    string fieldValue = TextHeader.Substring(fieldStartIx + fieldIdentifier.Length + 2,
                                                              fieldEndIx - (fieldStartIx + fieldIdentifier.Length + 2));
                    CreateField(fieldIdentifier, text, commonFieldDelimiters, fieldValue);
                }
            }
        }

        private int GetSmallestValidIx(int ix1, int ix2)
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

        private void CreateField(string fieldIdentifier, MT103SingleCustomerCreditTransferText text,
                                 CommonFieldDelimiters commonFieldDelimiters, string fieldValue)
        {
            switch (fieldIdentifier)
            {
                case MT103SingleCustomerCreditTransferText._13CKey:
                    text.Field13C = new Field13C(commonFieldDelimiters, fieldValue);
                    break;
                case MT103SingleCustomerCreditTransferText._20Key:
                    text.Field20 = new Field20(commonFieldDelimiters, fieldValue);
                    break;
                case MT103SingleCustomerCreditTransferText._23BKey:
                    text.Field23B = new Field23B(commonFieldDelimiters, fieldValue);
                    break;
                case MT103SingleCustomerCreditTransferText._23EKey:
                    text.Field23E = new Field23E(commonFieldDelimiters, fieldValue);
                    break;
                case MT103SingleCustomerCreditTransferText._32AKey:
                    text.Field32A = new Field32A(commonFieldDelimiters, fieldValue);
                    break;
                case MT103SingleCustomerCreditTransferText._33BKey:
                    text.Field33B = new Field33B(commonFieldDelimiters, fieldValue);
                    break;
                case MT103SingleCustomerCreditTransferText._50AKey:
                    text.Field50A = new Field50A(commonFieldDelimiters, fieldValue);
                    break;
                case MT103SingleCustomerCreditTransferText._50FKey:
                    text.Field50F = new Field50F(commonFieldDelimiters, fieldValue);
                    break;
                case MT103SingleCustomerCreditTransferText._50KKey:
                    text.Field50K = new Field50K(commonFieldDelimiters, fieldValue);
                    break;
                case MT103SingleCustomerCreditTransferText._52AKey:
                    text.Field52A = new Field52A(commonFieldDelimiters, fieldValue);
                    break;
                case MT103SingleCustomerCreditTransferText._52DKey:
                    text.Field52D = new Field52D(commonFieldDelimiters, fieldValue);
                    break;
                case MT103SingleCustomerCreditTransferText._59Key:
                    text.Field59 = new Field59(commonFieldDelimiters, fieldValue);
                    break;
                case MT103SingleCustomerCreditTransferText._59AKey:
                    text.Field59A = new Field59A(commonFieldDelimiters, fieldValue);
                    break;
                case MT103SingleCustomerCreditTransferText._59FKey:
                    text.Field59F = new Field59F(commonFieldDelimiters, fieldValue);
                    break;
                case MT103SingleCustomerCreditTransferText._70Key:
                    text.Field70 = new Field70(commonFieldDelimiters, fieldValue);
                    break;
                case MT103SingleCustomerCreditTransferText._71AKey:
                    text.Field71A = new Field71A(commonFieldDelimiters, fieldValue);
                    break;
                case MT103SingleCustomerCreditTransferText._71FKey:
                    text.Field71F = new Field71F(commonFieldDelimiters, fieldValue);
                    break;
                case MT103SingleCustomerCreditTransferText._72Key:
                    text.Field72 = new Field72(commonFieldDelimiters, fieldValue);
                    break;
                default:
                    throw new Exception("Program error, fieldIdentifier has an invalid value: '" + fieldIdentifier + ".");
            }
        }
    }
}
