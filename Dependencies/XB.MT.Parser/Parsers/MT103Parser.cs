using System;
using XB.MT.Common;
using XB.MT.Common.Model.Common;
using XB.MT.Common.Model.Tags.UserHeader;
using XB.MT.Parser.Model;
using XB.MT.Parser.Model.Common;
using XB.MT.Parser.Model.MessageHeader;
using static XB.MT.Parser.Model.MessageHeader.Trailer;

namespace XB.MT.Parser.Parsers
{
    public abstract class MT103Parser
    {

        protected void PopulateHeaders(MT103 mT103Instans, string message, MT103Version version)
        {
            mT103Instans.SetOriginalSWIFTmessageIfNull(message);
            PopulateBasicHeader(mT103Instans, message, version);

            ApplicationHeaderType applicationHeaderType = GetApplicationHeaderType(message, version);
            if (applicationHeaderType == ApplicationHeaderType.ApplicationHeaderInput)
            {
                PopulateApplicationHeaderInputMessage(mT103Instans, message, version);
            }
            else if (applicationHeaderType == ApplicationHeaderType.ApplicationHeaderOutput)
            {
                PopulateApplicationHeaderOutputMessage(mT103Instans, message, version);
            }

            PopulateUserHeader(mT103Instans, message, version);
            PopulateTrailer(mT103Instans, message, version);
        }
        protected void PopulateBasicHeader(MT103 mT103Instans, string message, MT103Version version)
        {
            mT103Instans.SetOriginalSWIFTmessageIfNull(message);
            // Prepared to handle different versions in different ways
            if (version == MT103Version.V_2020)
            {
                int basicHeaderStart = message.IndexOf("{1:");
                if (basicHeaderStart >= 0)
                {
                    mT103Instans.BasicHeader = new BasicHeader();
                    BasicHeader basicHeader = mT103Instans.BasicHeader;
                    CommonBlockDelimiters commonBlockFields = basicHeader.CommonBlockDelimiters;
                    commonBlockFields.SetDefaultStartOfBlockValues("1");

                    int basicHeaderEnd = message.IndexOf("}", basicHeaderStart);

                    string[] tags = { BasicHeader.AppIDKey,
                                      BasicHeader.ServiceIDKey,
                                      BasicHeader.LTAddressKey,
                                      BasicHeader.SessionNumberKey,
                                      BasicHeader.SequenceNumberKey };
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
            else
            {
                UnhandledVersion(version);
            }
        }

        private void SetBasicHeaderTags(string tag, BasicHeader basicHeader, string message,
                                       int basicHeaderStartIx, int basicHeaderEndIx)
        {
            switch (tag)
            {
                case BasicHeader.AppIDKey:
                    if (basicHeaderEndIx > basicHeaderStartIx + 3)
                    {
                        basicHeader.AppID = message.Substring(basicHeaderStartIx + 3, 1);
                    }
                    break;
                case BasicHeader.ServiceIDKey:
                    if (basicHeaderEndIx > basicHeaderStartIx + 5)
                    {
                        basicHeader.ServiceID = message.Substring(basicHeaderStartIx + 4, 2);
                    }
                    break;
                case BasicHeader.LTAddressKey:
                    if (basicHeaderEndIx > basicHeaderStartIx + 17)
                    {
                        basicHeader.LTAddress = message.Substring(basicHeaderStartIx + 6, 12);
                    }
                    break;
                case BasicHeader.SessionNumberKey:
                    if (basicHeaderEndIx > basicHeaderStartIx + 21)
                    {
                        basicHeader.SessionNumber = message.Substring(basicHeaderStartIx + 18, 4);
                    }
                    break;
                case BasicHeader.SequenceNumberKey:
                    if (basicHeaderEndIx > basicHeaderStartIx + 27)
                    {
                        basicHeader.SequenceNumber = message.Substring(basicHeaderStartIx + 22, 6);
                    }
                    break;
                default:
                    throw new Exception("Invalid basic header tag: '" + tag + "'.");
            }
        }

        protected void PopulateApplicationHeaderInputMessage(MT103 mT103Instans, string message, MT103Version version)
        {
            mT103Instans.SetOriginalSWIFTmessageIfNull(message);
            // Prepared to handle different versions in different ways
            if (version == MT103Version.V_2020)
            {
                int applicationHeaderStart = message.IndexOf("{2:");
                if (applicationHeaderStart >= 0)
                {
                    mT103Instans.ApplicationHeaderInputMessage = new ApplicationHeaderInputMessage();
                    ApplicationHeaderInputMessage applicationHeader = mT103Instans.ApplicationHeaderInputMessage;
                    CommonBlockDelimiters commonBlockFields = applicationHeader.CommonBlockDelimiters;
                    commonBlockFields.SetDefaultStartOfBlockValues("2");
                    int applicationHeaderEnd = message.IndexOf("}", applicationHeaderStart);

                    string[] tags = { ApplicationHeaderInputMessage.InputOutputIDKey,
                                      ApplicationHeaderInputMessage.MessageTypeKey,
                                      ApplicationHeaderInputMessage.DestinationAddressKey,
                                      ApplicationHeaderInputMessage.PriorityKey,
                                      ApplicationHeaderInputMessage.DeliveryMonitoringKey,
                                      ApplicationHeaderInputMessage.ObsolescencePeriodKey };
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
            else
            {
                UnhandledVersion(version);
            }
        }

        protected void PopulateApplicationHeaderOutputMessage(MT103 mT103Instans, string message, MT103Version version)
        {
            mT103Instans.SetOriginalSWIFTmessageIfNull(message);
            // Prepared to handle different versions in different ways
            if (version == MT103Version.V_2020)
            {
                int applicationHeaderStart = message.IndexOf("{2:");
                if (applicationHeaderStart >= 0)
                {
                    mT103Instans.ApplicationHeaderOutputMessage = new ApplicationHeaderOutputMessage();
                    ApplicationHeaderOutputMessage applicationHeader = mT103Instans.ApplicationHeaderOutputMessage;
                    CommonBlockDelimiters commonBlockFields = applicationHeader.CommonBlockDelimiters;
                    commonBlockFields.SetDefaultStartOfBlockValues("2");
                    int applicationHeaderEnd = message.IndexOf("}", applicationHeaderStart);
                    if (applicationHeaderEnd > 0)
                    {
                        commonBlockFields.SetDefaultValue(CommonBlockDelimiter.EndOfBlockDelimiter);
                    }

                    string[] tags = { ApplicationHeaderOutputMessage.InputOutputIDKey,
                                      ApplicationHeaderOutputMessage.MessageTypeKey, 
                                      ApplicationHeaderOutputMessage.InputTimeKey,
                                      ApplicationHeaderOutputMessage.MessageInputReferenceKey,
                                      ApplicationHeaderOutputMessage.OutputDateKey,
                                      ApplicationHeaderOutputMessage.OutputTimeKey,
                                      ApplicationHeaderOutputMessage.PriorityKey };
                    foreach (var tag in tags)
                    {
                        SetApplicationHeaderOutputTags(tag, applicationHeader, message, applicationHeaderStart, applicationHeaderEnd);
                    }
                }
            }
            else
            {
                UnhandledVersion(version);
            }
        }

        private ApplicationHeaderType GetApplicationHeaderType(string message, MT103Version version)
        {
            ApplicationHeaderType applicationHeaderType = ApplicationHeaderType.DoNotExist;
            if (version == MT103Version.V_2020)
            {
                int applicationHeaderStartIx = message.IndexOf("{2:");
                if (applicationHeaderStartIx >= 0)
                {
                    if (message.Length > applicationHeaderStartIx + 3)
                    {
                        string InputOutputID = message.Substring(applicationHeaderStartIx + 3, 1);
                        if (InputOutputID.Equals("I", StringComparison.InvariantCultureIgnoreCase))
                        {
                            applicationHeaderType = ApplicationHeaderType.ApplicationHeaderInput;
                        }
                        else if (InputOutputID.Equals("O"))
                        {
                            applicationHeaderType = ApplicationHeaderType.ApplicationHeaderOutput;
                        }
                        else
                        {
                            throw new Exception("Application header InputOutputID has an invalid value: '" + InputOutputID +
                                                "', valid values are 'I' and 'O'");
                        }
                    }
                    else
                    {
                        throw new Exception("Application header InputOutputID could not be found, message is too short");
                    }
                }
            }
            else
            {
                UnhandledVersion(version);
            }
            return applicationHeaderType;
        }

        private void SetApplicationHeaderInputTags(string tag, ApplicationHeaderInputMessage applicationHeader,
                                                          string message, int applicationHeaderStartIx, int applicationHeaderEndIx)
        {
            switch (tag)
            {
                case ApplicationHeaderInputMessage.InputOutputIDKey:
                    if (applicationHeaderEndIx > applicationHeaderStartIx + 3)
                    {
                        applicationHeader.InputOutputID = message.Substring(applicationHeaderStartIx + 3, 1);
                    }
                    break;
                case ApplicationHeaderInputMessage.MessageTypeKey:
                    if (applicationHeaderEndIx > applicationHeaderStartIx + 6)
                    {
                        applicationHeader.MessageType = message.Substring(applicationHeaderStartIx + 4, 3);
                    }
                    break;
                case ApplicationHeaderInputMessage.DestinationAddressKey:
                    if (applicationHeaderEndIx > applicationHeaderStartIx + 18)
                    {
                        applicationHeader.DestinationAddress = message.Substring(applicationHeaderStartIx + 7, 12);
                    }
                    break;
                case ApplicationHeaderInputMessage.PriorityKey:
                    if (applicationHeaderEndIx > applicationHeaderStartIx + 19)
                    {
                        applicationHeader.Priority = message.Substring(applicationHeaderStartIx + 19, 1);
                    }
                    break;
                case ApplicationHeaderInputMessage.DeliveryMonitoringKey:
                    if (applicationHeaderEndIx > applicationHeaderStartIx + 20)
                    {
                        applicationHeader.DeliveryMonitoring = message.Substring(applicationHeaderStartIx + 20, 1);
                    }
                    break;
                case ApplicationHeaderInputMessage.ObsolescencePeriodKey:
                    if (applicationHeaderEndIx > applicationHeaderStartIx + 23)
                    {
                        applicationHeader.ObsolescencePeriod = message.Substring(applicationHeaderStartIx + 21, 3);
                    }
                    break;
                default:
                    throw new Exception("Invalid application header tag: '" + tag + "'.");
            }
        }

        private void SetApplicationHeaderOutputTags(string tag, ApplicationHeaderOutputMessage applicationHeader,
                                                          string message, int applicationHeaderStartIx, int applicationHeaderEndIx)
        {
            switch (tag)
            {
                case ApplicationHeaderOutputMessage.InputOutputIDKey:
                    if (applicationHeaderEndIx > applicationHeaderStartIx + 3)
                    {
                        applicationHeader.InputOutputID = message.Substring(applicationHeaderStartIx + 3, 1);
                    }
                    break;
                case ApplicationHeaderOutputMessage.MessageTypeKey:
                    if (applicationHeaderEndIx > applicationHeaderStartIx + 6)
                    {
                        applicationHeader.MessageType = message.Substring(applicationHeaderStartIx + 4, 3);
                    }
                    break;
                case ApplicationHeaderOutputMessage.InputTimeKey:
                    if (applicationHeaderEndIx > applicationHeaderStartIx + 10)
                    {
                        applicationHeader.InputTime = message.Substring(applicationHeaderStartIx + 7, 4);
                    }
                    break;
                case ApplicationHeaderOutputMessage.MessageInputReferenceKey:
                    if (applicationHeaderEndIx > applicationHeaderStartIx + 38)
                    {
                        applicationHeader.MessageInputReference = message.Substring(applicationHeaderStartIx + 11, 28);
                    }
                    break;
                case ApplicationHeaderOutputMessage.OutputDateKey:
                    if (applicationHeaderEndIx > applicationHeaderStartIx + 44)
                    {
                        applicationHeader.OutputDate = message.Substring(applicationHeaderStartIx + 39, 6);
                    }
                    break;
                case ApplicationHeaderOutputMessage.OutputTimeKey:
                    if (applicationHeaderEndIx > applicationHeaderStartIx + 48)
                    {
                        applicationHeader.OutputTime = message.Substring(applicationHeaderStartIx + 45, 4);
                    }
                    break;
                case ApplicationHeaderOutputMessage.PriorityKey:
                    if (applicationHeaderEndIx > applicationHeaderStartIx + 49)
                    {
                        applicationHeader.Priority = message.Substring(applicationHeaderStartIx + 49, 1);
                    }
                    break;
                default:
                    throw new Exception("Invalid application header tag: '" + tag + "'.");
            }
        }

        protected void PopulateUserHeader(MT103 mT103Instans, string message, MT103Version version)
        {
            mT103Instans.SetOriginalSWIFTmessageIfNull(message);
            // Prepared to handle different versions in different ways
            if (version == MT103Version.V_2020)
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

                    string[] tags = new string[] { UserHeader._103Key, UserHeader._113Key, UserHeader._108Key, UserHeader._119Key,
                                                   UserHeader._423Key, UserHeader._106Key, UserHeader._424Key, UserHeader._111Key,
                                                   UserHeader._121Key, UserHeader._115Key, UserHeader._165Key, UserHeader._433Key,
                                                   UserHeader._434Key };
                    foreach (var tag in tags)
                    {
                        HandleUserHeaderTag(tag, userHeaderMessage, userHeader);
                    }
                }
            }
            else
            {
                UnhandledVersion(version);
            }
        }

        private void HandleUserHeaderTag(string tagId, string blockMessage, UserHeader userHeader)
        {
            CommonBlockDelimiters commonTagDelimiters = new CommonBlockDelimiters();
            string tagValue = HandleTag(tagId, blockMessage, commonTagDelimiters);
            if (tagValue != null)
            {
                CreateUserHeaderTag(tagId, userHeader, commonTagDelimiters, tagValue);
            }
        }

        private string HandleTag(string tagId, string blockMessage, CommonBlockDelimiters commonTagDelimiters)
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

        private void CreateUserHeaderTag(string tagId, UserHeader userHeader, CommonBlockDelimiters commonTagDelimiters, string tagValue)
        {
            switch (tagId)
            {
                case UserHeader._103Key:
                    userHeader.Tag103_ServiceIdentifier = new Tag103ServiceIdentifier(commonTagDelimiters, tagValue);
                    break;
                case UserHeader._113Key:
                    userHeader.Tag113_BankingPriority = new Tag113BankingPriority(commonTagDelimiters, tagValue);
                    break;
                case UserHeader._108Key:
                    userHeader.Tag108_MessageUserReference = new Tag108MessageUserReference(commonTagDelimiters, tagValue);
                    break;
                case UserHeader._119Key:
                    userHeader.Tag119_ValidationFlag = new Tag119ValidationFlag(commonTagDelimiters, tagValue);
                    break;
                case UserHeader._423Key:
                    userHeader.Tag423_BalanceCheckpointDateAndTime = new Tag423BalanceCheckpointDateAndTime(commonTagDelimiters, tagValue);
                    break;
                case UserHeader._106Key:
                    userHeader.Tag106_MessageInputReference = new Tag106MessageInputReference(commonTagDelimiters, tagValue);
                    break;
                case UserHeader._424Key:
                    userHeader.Tag424_RelatedReference = new Tag424RelatedReference(commonTagDelimiters, tagValue);
                    break;
                case UserHeader._111Key:
                    userHeader.Tag111_ServiceTypeIdentifier = new Tag111ServiceTypeIdentifier(commonTagDelimiters, tagValue);
                    break;
                case UserHeader._121Key:
                    userHeader.Tag121_UniqueEndToEndTransactionReference =
                        new Tag121UniqueEndToEndTransactionReference(commonTagDelimiters, tagValue);
                    break;
                case UserHeader._115Key:
                    userHeader.Tag115_AddresseeInformation = new Tag115AddresseeInformation(commonTagDelimiters, tagValue);
                    break;
                case UserHeader._165Key:
                    userHeader.Tag165_PaymentReleaseInformationReceiver = new Tag165PaymentReleaseInformationReceiver(commonTagDelimiters, tagValue);
                    break;
                case UserHeader._433Key:
                    userHeader.Tag433_SanctionsScreeningInformationForTheReceiver =
                        new Tag433SanctionsScreeningInformationForTheReceiver(commonTagDelimiters, tagValue);
                    break;
                case UserHeader._434Key:
                    userHeader.Tag434_PaymentControlsInformationForReceiver =
                        new Tag434PaymentControlsInformationForReceiver(commonTagDelimiters, tagValue);
                    break;
                default:
                    throw new Exception("Program error, tagId has an invalid value: '" + tagId + ".");
            }
        }

        protected void PopulateTrailer(MT103 mT103Instans, string message, MT103Version version)
        {
            mT103Instans.SetOriginalSWIFTmessageIfNull(message);
            // Prepared to handle different versions in different ways
            if (version == MT103Version.V_2020)
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

                    string[] tags = new string[] { Trailer.CHKKey, Trailer.TNGKey, Trailer.PDEKey, Trailer.DLMKey, 
                                                   Trailer.MRFKey, Trailer.PDMKey, Trailer.SYSKey };
                    foreach (var tag in tags)
                    {
                        HandleTrailerTag(tag, trailerMessage, trailer);
                    }
                }
            }
            else
            {
                UnhandledVersion(version);
            }
        }



        private void HandleTrailerTag(string tagId, string blockMessage, Trailer trailer)
        {
            CommonBlockDelimiters commonTagDelimiters = new CommonBlockDelimiters();
            string tagValue = HandleTag(tagId, blockMessage, commonTagDelimiters);
            if (tagValue != null)
            {
                CreateTrailerTag(tagId, trailer, commonTagDelimiters, tagValue);
            }
        }


        private void CreateTrailerTag(string tagId, Trailer trailer, CommonBlockDelimiters commonTagDelimiters, string tagValue)
        {
            switch (tagId)
            {
                case Trailer.CHKKey:
                    trailer.Tag_Checksum = new TagChecksum(commonTagDelimiters, tagValue);
                    break;
                case Trailer.TNGKey:
                    trailer.Tag_TestAndTrainingMessage = new TagTestAndTrainingMessage(commonTagDelimiters);
                    break;
                case Trailer.PDEKey:
                    trailer.Tag_PossibleDuplicateEmission = new TagPossibleDuplicateEmission(commonTagDelimiters, tagValue);
                    break;
                case Trailer.DLMKey:
                    trailer.Tag_DelayedMessage = new TagDelayedMessage(commonTagDelimiters);
                    break;
                case Trailer.MRFKey:
                    trailer.Tag_MessageReference = new TagMessageReference(commonTagDelimiters, tagValue);
                    break;
                case Trailer.PDMKey:
                    trailer.Tag_PossibleDuplicateMessage = new TagPossibleDuplicateMessage(commonTagDelimiters, tagValue);
                    break;
                case Trailer.SYSKey:
                    trailer.Tag_SystemOriginatedMessage = new TagSystemOriginatedMessage(commonTagDelimiters, tagValue);
                    break;
                default:
                    throw new Exception("Program error, tagId has an invalid value: '" + tagId + ".");
            }
        }


        protected void UnhandledVersion(MT103Version version)
        {
            throw new Exception("The MT 103 Single Customer Credit Transfer message has an unhandled version: " + version +
                                ", valid version(s) is/are: " + MT103Version.V_2020 + ".");
        }


    }


}
