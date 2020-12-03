using System;
using System.Collections.Generic;
using System.Text;
using XB.MT.Parser.Model.Common;

namespace XB.MT.Parser.Model.MessageHeader
{
    public class UserHeader : BlockHeader
    {

        public Tag103ServiceIdentifier Tag103_ServiceIdentifier { get; set; }
        public Tag113BankingPriority Tag113_BankingPriority { get; set; }
        public Tag108MessageUserReference Tag108_MessageUserReference { get; set; }
        public Tag119ValidationFlag Tag119_ValidationFlag { get; set; }
        public Tag423BalanceCheckpointDateAndTime Tag423_BalanceCheckpointDateAndTime { get; set; }
        public Tag106MessageInputReference Tag106_MessageInputReference { get; set; }
        public Tag424RelatedReference Tag424_RelatedReference { get; set; }
        public Tag111ServiceTypeIdentifier Tag111_ServiceTypeIdentifier { get; set; }
        public Tag121UniqueEndToEndTransactionReference Tag121_UniqueEndToEndTransactionReference { get; set; }
        public Tag115AddresseeInformation Tag115_AddresseeInformation { get; set; }
        public Tag165PaymentReleaseInformationReceiver Tag165_PaymentReleaseInformationReceiver { get; set; }
        public Tag433SanctionsScreeningInformationForTheReceiver Tag433_SanctionsScreeningInformationForTheReceiver { get; set; }
        public Tag434PaymentControlsInformationForReceiver Tag434_PaymentControlsInformationForReceiver { get; set; }


        public class Tag103ServiceIdentifier : TagHeader
        {
            public string ServiceIdentifier { get; set; }
            public Tag103ServiceIdentifier(CommonBlockDelimiters commonTagDelimiters, string serviceIdentifier) : base(commonTagDelimiters)
            {
                ServiceIdentifier = serviceIdentifier;
            }
        }
        public class Tag113BankingPriority : TagHeader
        {
            public Tag113BankingPriority(CommonBlockDelimiters commonTagDelimiters, string tagValue) : base(commonTagDelimiters)
            {
                BankingPriority = tagValue;
            }

            public string BankingPriority { get; set; }
        }
        public class Tag108MessageUserReference : TagHeader
        {
            public Tag108MessageUserReference(CommonBlockDelimiters commonTagDelimiters, string tagValue) : base(commonTagDelimiters)
            {
                MessageUserReference = tagValue;
            }

            public string MessageUserReference { get; set; }
        }
        public class Tag119ValidationFlag : TagHeader
        {
            public Tag119ValidationFlag(CommonBlockDelimiters commonTagDelimiters, string tagValue) : base(commonTagDelimiters)
            {
                ValidationFlag = tagValue;
            }

            public string ValidationFlag { get; set; }
        }
        public class Tag423BalanceCheckpointDateAndTime : TagHeader
        {
            public Tag423BalanceCheckpointDateAndTime(CommonBlockDelimiters commonTagDelimiters, string tagValue) : base(commonTagDelimiters)
            {
                BalanceCheckpointDateAndTime = tagValue;
            }

            public string BalanceCheckpointDateAndTime { get; set; }
        }
        public class Tag106MessageInputReference : TagHeader
        {
            public Tag106MessageInputReference(CommonBlockDelimiters commonTagDelimiters, string tagValue) : base(commonTagDelimiters)
            {
                MessageInputReference = tagValue;
            }

            public string MessageInputReference { get; set; }
        }
        public class Tag424RelatedReference : TagHeader
        {
            public Tag424RelatedReference(CommonBlockDelimiters commonTagDelimiters, string tagValue) : base(commonTagDelimiters)
            {
                RelatedReference = tagValue;
            }

            public string RelatedReference { get; set; }
        }
        public class Tag111ServiceTypeIdentifier : TagHeader
        {
            public Tag111ServiceTypeIdentifier(CommonBlockDelimiters commonTagDelimiters, string tagValue) : base(commonTagDelimiters)
            {
                ServiceTypeIdentifier = tagValue;
            }

            public string ServiceTypeIdentifier { get; set; }
        }
        public class Tag121UniqueEndToEndTransactionReference : TagHeader
        {
            public Tag121UniqueEndToEndTransactionReference(CommonBlockDelimiters commonTagDelimiters, string tagValue) : base(commonTagDelimiters)
            {
                UniqueEndToEndTransactionReference = tagValue;
            }

            public string UniqueEndToEndTransactionReference { get; set; }
        }
        public class Tag115AddresseeInformation : TagHeader
        {
            public Tag115AddresseeInformation(CommonBlockDelimiters commonTagDelimiters, string tagValue) : base(commonTagDelimiters)
            {
                AddresseeInformation = tagValue;
            }

            public string AddresseeInformation { get; set; }
        }
        public class Tag165PaymentReleaseInformationReceiver : TagHeader
        {
            public Tag165PaymentReleaseInformationReceiver(CommonBlockDelimiters commonTagDelimiters, string tagValue) : base(commonTagDelimiters)
            {
                PaymentReleaseInformationReceiver = tagValue;
            }

            public string PaymentReleaseInformationReceiver { get; set; }
        }
        public class Tag433SanctionsScreeningInformationForTheReceiver : TagHeader
        {
            public Tag433SanctionsScreeningInformationForTheReceiver(CommonBlockDelimiters commonTagDelimiters, string tagValue) : 
                                                                     base(commonTagDelimiters)
            {
                SanctionsScreeningInformationForTheReceiver = tagValue;
            }

            public string SanctionsScreeningInformationForTheReceiver { get; set; }
        }
        public class Tag434PaymentControlsInformationForReceiver : TagHeader
        {
            public Tag434PaymentControlsInformationForReceiver(CommonBlockDelimiters commonTagDelimiters, string tagValue) : 
                                                               base(commonTagDelimiters)
            {
                PaymentControlsInformationForReceiver = tagValue;
            }

            public string PaymentControlsInformationForReceiver { get; set; }
        }
    }
}
