using System;
using XB.MT.Parser.Model;
using XB.MT.Parser.Model.Common;
using XB.MT.Parser.Model.MessageHeader;
using XB.MT.Parser.Model.Text.MT103;
using XB.MT.Parser.Model.Text.MT103.Fields;
using static XB.MT.Parser.Model.MessageHeader.Trailer;
using static XB.MT.Parser.Model.MessageHeader.UserHeader;

namespace XB.MT.Parser.Parsers
{
    public static class MT103SingleCustomerCreditTransferParser
    {
        public static MT103SingleCustomerCreditTransferModel ParseMessage(string message)
        {
            //ValidateMT103Message(message);
            MT103SingleCustomerCreditTransferModel mT103Instans = new MT103SingleCustomerCreditTransferModel();
            PopulateBasicHeader(mT103Instans, message);
            PopulateApplicationHeaderInputMessage(mT103Instans, message);
            PopulateUserHeader(mT103Instans, message);
            PopulateText(mT103Instans, message);
            PopulateTrailer(mT103Instans, message);

            return mT103Instans;
        }

        private static void PopulateBasicHeader(MT103SingleCustomerCreditTransferModel mT103Instans, string message)
        {
            int basicHeaderStart = message.IndexOf("{1:");
            if (basicHeaderStart >= 0)
            {
                mT103Instans.BasicHeader = new BasicHeader();
                BasicHeader basicHeader = mT103Instans.BasicHeader;
                CommonBlockDelimiters commonBlockFields = basicHeader.CommonBlockDelimiters;
                commonBlockFields.SetDefaultStartOfBlockValues("1");

                int basicHeaderEnd = message.IndexOf("}", basicHeaderStart);

                string[] tags = { "AppID", "ServiceID", "LTAddress", "SessionNumber", "SequenceNumber" };
                foreach (var tag in tags)
                {
                    SetBasicHeaderTags(tag, basicHeader, message, basicHeaderStart, basicHeaderEnd);
                }

                if (basicHeaderEnd > 0)
                {
                    commonBlockFields.SetDefaultValue(CommonBlockDelimiter.EndOfBlockDelimiter);
                }
            }
        }

        private static void SetBasicHeaderTags(string tag, BasicHeader basicHeader, string message, int basicHeaderStartIx, int basicHeaderEndIx)
        {
            switch (tag)
            {
                case "AppID":
                    if (basicHeaderEndIx > basicHeaderStartIx + 3)
                    {
                        basicHeader.AppID = message.Substring(basicHeaderStartIx + 3, 1);
                    }
                    break;
                case "ServiceID":
                    if (basicHeaderEndIx > basicHeaderStartIx + 5)
                    {
                        basicHeader.ServiceID = message.Substring(basicHeaderStartIx + 4, 2);
                    }
                    break;
                case "LTAddress":
                    if (basicHeaderEndIx > basicHeaderStartIx + 17)
                    {
                        basicHeader.LTAddress = message.Substring(basicHeaderStartIx + 6, 12);
                    }
                    break;
                case "SessionNumber":
                    if (basicHeaderEndIx > basicHeaderStartIx + 21)
                    {
                        basicHeader.SessionNumber = message.Substring(basicHeaderStartIx + 18, 4);
                    }
                    break;
                case "SequenceNumber":
                    if (basicHeaderEndIx > basicHeaderStartIx + 27)
                    {
                        basicHeader.SequenceNumber = message.Substring(basicHeaderStartIx + 22, 6);
                    }
                    break;
                default:
                    throw new Exception("Invalid basic header tag: '" + tag + "'.");
            }
        }

        private static void PopulateApplicationHeaderInputMessage(MT103SingleCustomerCreditTransferModel mT103Instans, string message)
        {
            int applicationHeaderStart = message.IndexOf("{2:");
            if (applicationHeaderStart >= 0)
            {
                mT103Instans.ApplicationHeaderInputMessage = new ApplicationHeaderInputMessage();
                ApplicationHeaderInputMessage applicationHeader = mT103Instans.ApplicationHeaderInputMessage;
                CommonBlockDelimiters commonBlockFields = applicationHeader.CommonBlockDelimiters;
                commonBlockFields.SetDefaultStartOfBlockValues("2");

                int applicationHeaderEnd = message.IndexOf("}", applicationHeaderStart);
                string[] tags = { "InputOutputID", "MessageType", "DestinationAddress",
                                  "Priority", "DeliveryMonitoring", "ObsolescencePeriod" };
                foreach (var tag in tags)
                {
                    SetApplicationHeaderInputTags(tag, applicationHeader, message, applicationHeaderStart, applicationHeaderEnd);
                }

                if (applicationHeaderEnd > 0)
                {
                    commonBlockFields.SetDefaultValue(CommonBlockDelimiter.EndOfBlockDelimiter);
                }
            }
        }

        private static void SetApplicationHeaderInputTags(string tag, ApplicationHeaderInputMessage applicationHeader,
                                                          string message, int applicationHeaderStartIx, int applicationHeaderEndIx)
        {
            switch (tag)
            {
                case "InputOutputID":
                    if (applicationHeaderEndIx > applicationHeaderStartIx + 3)
                    {
                        applicationHeader.InputOutputID = message.Substring(applicationHeaderStartIx + 3, 1);
                    }
                    break;
                case "MessageType":
                    if (applicationHeaderEndIx > applicationHeaderStartIx + 6)
                    {
                        applicationHeader.MessageType = message.Substring(applicationHeaderStartIx + 4, 3);
                    }
                    break;
                case "DestinationAddress":
                    if (applicationHeaderEndIx > applicationHeaderStartIx + 18)
                    {
                        applicationHeader.DestinationAddress = message.Substring(applicationHeaderStartIx + 7, 12);
                    }
                    break;
                case "Priority":
                    if (applicationHeaderEndIx > applicationHeaderStartIx + 19)
                    {
                        applicationHeader.Priority = message.Substring(applicationHeaderStartIx + 19, 1);
                    }
                    break;
                case "DeliveryMonitoring":
                    if (applicationHeaderEndIx > applicationHeaderStartIx + 20)
                    {
                        applicationHeader.DeliveryMonitoring = message.Substring(applicationHeaderStartIx + 20, 1);
                    }
                    break;
                case "ObsolescencePeriod":
                    if (applicationHeaderEndIx > applicationHeaderStartIx + 23)
                    {
                        applicationHeader.ObsolescencePeriod = message.Substring(applicationHeaderStartIx + 21, 3);
                    }
                    break;
                default:
                    throw new Exception("Invalid application header tag: '" + tag + "'.");
            }
        }

        private static void PopulateUserHeader(MT103SingleCustomerCreditTransferModel mT103Instans, string message)
        {
            int userHeaderStart = message.IndexOf("{3:");
            if (userHeaderStart >= 0)
            {
                mT103Instans.UserHeader = new UserHeader();
                UserHeader userHeader = mT103Instans.UserHeader;
                CommonBlockDelimiters commonBlockDelimiters = userHeader.CommonBlockDelimiters;
                commonBlockDelimiters.SetDefaultStartOfBlockValues("3");

                string userHeaderMessage = "";
                int userHeaderEnd = message.IndexOf("}}", userHeaderStart);
                if (userHeaderEnd > -1)
                {
                    userHeaderEnd++;
                    commonBlockDelimiters.SetDefaultValue(CommonBlockDelimiter.EndOfBlockDelimiter);
                    userHeaderMessage = message.Substring(userHeaderStart, userHeaderEnd - userHeaderStart + 1);
                }
                else
                {
                    // Will not be set if there is no end EndOfBlockIndicator for the UserHeader?
                    userHeaderMessage = message;
                }

                string[] tags = new string[] { "103", "113", "108", "119", "423", "106", "424",
                                               "111", "121", "115", "165", "433", "434" };
                foreach (var tag in tags)
                {
                    HandleUserHeaderTag(tag, userHeaderMessage, userHeader);
                }
            }
        }


        private static void PopulateText(MT103SingleCustomerCreditTransferModel mT103Instans, string message)
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
                                                           "52A", "59", "70", "71A", "71F" };
                foreach (var fieldIdentifier in fieldIdentifiers)
                {
                    HandleField(fieldIdentifier, textMessage, text);
                }
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
                int fieldEndIx1 = textMessage.IndexOf("\r\n:", fieldStartIx + fieldIdentifier.Length + 2);  // Start of next field
                int fieldEndIx2 = textMessage.IndexOf("\r\n-}", fieldStartIx + fieldIdentifier.Length + 2); // End of block Text
                if (fieldEndIx1 < 0 && fieldEndIx2 < 0)
                {
                    throw new Exception("End of field ('\r\n:') and end of block ('\r\n-}') not found in textMessage after index " +
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

        private static void PopulateTrailer(MT103SingleCustomerCreditTransferModel mT103Instans, string message)
        {
            int trailerStart = message.IndexOf("{5:");
            if (trailerStart >= 0)
            {
                mT103Instans.Trailer = new Trailer();
                Trailer trailer = mT103Instans.Trailer;
                CommonBlockDelimiters commonBlockFields = trailer.CommonBlockDelimiters;
                commonBlockFields.SetDefaultStartOfBlockValues("5");

                string trailerMessage = "";
                int trailerEnd = message.IndexOf("}}", trailerStart);
                if (trailerEnd > -1)
                {
                    trailerEnd++;
                    commonBlockFields.SetDefaultValue(CommonBlockDelimiter.EndOfBlockDelimiter);
                    trailerMessage = message.Substring(trailerStart, trailerEnd - trailerStart + 1);
                }
                else
                {
                    // Will not be set if there is no end EndOfBlockIndicator for the Trailer?
                    trailerMessage = message;
                }

                string[] tags = new string[] { "CHK", "TNG", "PDE", "DLM", "MRF", "PDM", "SYS" };
                foreach (var tag in tags)
                {
                    HandleTrailerTag(tag, trailerMessage, trailer);
                }
            }
        }


        private static string HandleTag(string tagId, string blockMessage, CommonBlockDelimiters commonTagDelimiters)
        {
            string tagValue = null;
            int tagStartIx = blockMessage.IndexOf("{" + tagId + ":");
            if (tagStartIx > -1)
            {
                commonTagDelimiters.SetDefaultStartOfBlockValues(tagId);
                int tagEndIx = blockMessage.IndexOf("}", tagStartIx);
                if (tagEndIx > -1)
                {
                    commonTagDelimiters.SetDefaultValue(CommonBlockDelimiter.EndOfBlockDelimiter);
                    tagValue = blockMessage.Substring(tagStartIx + tagId.Length + 2, tagEndIx - (tagStartIx + tagId.Length + 2));
                }
            }
            return tagValue;
        }

        private static void HandleTrailerTag(string tagId, string blockMessage, Trailer trailer)
        {
            CommonBlockDelimiters commonTagDelimiters = new CommonBlockDelimiters();
            string tagValue = HandleTag(tagId, blockMessage, commonTagDelimiters);
            if (tagValue != null)
            {
                CreateTrailerTag(tagId, trailer, commonTagDelimiters, tagValue);
            }
        }


        private static void HandleUserHeaderTag(string tagId, string blockMessage, UserHeader userHeader)
        {
            CommonBlockDelimiters commonTagDelimiters = new CommonBlockDelimiters();
            string tagValue = HandleTag(tagId, blockMessage, commonTagDelimiters);
            if (tagValue != null)
            {
                CreateUserHeaderTag(tagId, userHeader, commonTagDelimiters, tagValue);
            }
        }

        private static void CreateUserHeaderTag(string tagId, UserHeader userHeader, CommonBlockDelimiters commonTagDelimiters, string tagValue)
        {
            switch (tagId)
            {
                case "103":
                    userHeader.Tag103_ServiceIdentifier = new Tag103ServiceIdentifier(commonTagDelimiters, tagValue);
                    break;
                case "113":
                    userHeader.Tag113_BankingPriority = new Tag113BankingPriority(commonTagDelimiters, tagValue);
                    break;
                case "108":
                    userHeader.Tag108_MessageUserReference = new Tag108MessageUserReference(commonTagDelimiters, tagValue);
                    break;
                case "119":
                    userHeader.Tag119_ValidationFlag = new Tag119ValidationFlag(commonTagDelimiters, tagValue);
                    break;
                case "423":
                    userHeader.Tag423_BalanceCheckpointDateAndTime = new Tag423BalanceCheckpointDateAndTime(commonTagDelimiters, tagValue);
                    break;
                case "106":
                    userHeader.Tag106_MessageInputReference = new Tag106MessageInputReference(commonTagDelimiters, tagValue);
                    break;
                case "424":
                    userHeader.Tag424_RelatedReference = new Tag424RelatedReference(commonTagDelimiters, tagValue);
                    break;
                case "111":
                    userHeader.Tag111_ServiceTypeIdentifier = new Tag111ServiceTypeIdentifier(commonTagDelimiters, tagValue);
                    break;
                case "121":
                    userHeader.Tag121_UniqueEndToEndTransactionReference = 
                        new Tag121UniqueEndToEndTransactionReference(commonTagDelimiters, tagValue);
                    break;
                case "115":
                    userHeader.Tag115_AddresseeInformation = new Tag115AddresseeInformation(commonTagDelimiters, tagValue);
                    break;
                case "165":
                    userHeader.Tag165_PaymentReleaseInformationReceiver = new Tag165PaymentReleaseInformationReceiver(commonTagDelimiters, tagValue);
                    break;
                case "433":
                    userHeader.Tag433_SanctionsScreeningInformationForTheReceiver = 
                        new Tag433SanctionsScreeningInformationForTheReceiver(commonTagDelimiters, tagValue);
                    break;
                case "434":
                    userHeader.Tag434_PaymentControlsInformationForReceiver = 
                        new Tag434PaymentControlsInformationForReceiver(commonTagDelimiters, tagValue);
                    break;
                default:
                    throw new Exception("Program error, tagId has an invalid value: '" + tagId + ".");
            }
        }
        private static void CreateTrailerTag(string tagId, Trailer trailer, CommonBlockDelimiters commonTagDelimiters, string tagValue)
        {
            switch (tagId)
            {
                case "CHK":
                    trailer.Tag_Checksum = new TagChecksum(commonTagDelimiters, tagValue);
                    break;
                case "TNG":
                    trailer.Tag_TestAndTrainingMessage = new TagTestAndTrainingMessage(commonTagDelimiters);
                    break;
                case "PDE":
                    trailer.Tag_PossibleDuplicateEmission = new TagPossibleDuplicateEmission(commonTagDelimiters, tagValue);
                    break;
                case "DLM":
                    trailer.Tag_DelayedMessage = new TagDelayedMessage(commonTagDelimiters);
                    break;
                case "MRF":
                    trailer.Tag_MessageReference = new TagMessageReference(commonTagDelimiters, tagValue);
                    break;
                case "PDM":
                    trailer.Tag_PossibleDuplicateMessage = new TagPossibleDuplicateMessage(commonTagDelimiters, tagValue);
                    break;
                case "SYS":
                    trailer.Tag_SystemOriginatedMessage = new TagSystemOriginatedMessage(commonTagDelimiters, tagValue);
                    break;
                default:
                    throw new Exception("Program error, tagId has an invalid value: '" + tagId + ".");
            }
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
