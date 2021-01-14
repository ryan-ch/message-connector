using MT103XUnitTestProject.Common;
using XB.MT.Parser.Model.MessageHeader;
using Xunit;

namespace MT103XUnitTestProject.MessageHeader
{
    internal class UserHeaderUnitTest
    {
        internal static void ValidateUserHeader(UserHeader userHeader, string tag103serviceIdentifier,
                                                               string tag106messageInputReference,
                                                               string tag108messageUserReference,
                                                               string tag111serviceTypeIdentifier,
                                                               string tag113bankingPriority,
                                                               string tag115addresseeInformation,
                                                               string tag119validationFlag,
                                                               string tag121uniqueEndToEndTransactionReference,
                                                               string tag165paymentReleaseInformationReceiver,
                                                               string tag423balanceCheckpointDateAndTime,
                                                               string tag424relatedReference,
                                                               string tag433sanctionsScreeningInformationForTheReceiver,
                                                               string tag434paymentControlsInformationForReceiver)
        {
            Assert.NotNull(userHeader);
            CommonBlockDelimitersUnitTest.ValidateCommonBlockDelimiters(userHeader.CommonBlockDelimiters, "3");
            if (tag103serviceIdentifier == null)
            {
                Assert.Null(userHeader.Tag103_ServiceIdentifier);
            }
            else
            {
                ValidateTag103(userHeader.Tag103_ServiceIdentifier, tag103serviceIdentifier);
            }

            if (tag106messageInputReference == null)
            {
                Assert.Null(userHeader.Tag106_MessageInputReference);
            }
            else
            {
                ValidateTag106(userHeader.Tag106_MessageInputReference, tag106messageInputReference);
            }

            if (tag108messageUserReference == null)
            {
                Assert.Null(userHeader.Tag108_MessageUserReference);
            }
            else
            {
                ValidateTag108(userHeader.Tag108_MessageUserReference, tag108messageUserReference);
            }

            if (tag111serviceTypeIdentifier == null)
            {
                Assert.Null(userHeader.Tag111_ServiceTypeIdentifier);
            }
            else
            {
                ValidateTag111(userHeader.Tag111_ServiceTypeIdentifier, tag111serviceTypeIdentifier);
            }

            if (tag113bankingPriority == null)
            {
                Assert.Null(userHeader.Tag113_BankingPriority);
            }
            else
            {
                ValidateTag113(userHeader.Tag113_BankingPriority, tag113bankingPriority);
            }

            if (tag115addresseeInformation == null)
            {
                Assert.Null(userHeader.Tag115_AddresseeInformation);
            }
            else
            {
                ValidateTag115(userHeader.Tag115_AddresseeInformation, tag115addresseeInformation);
            }

            if (tag119validationFlag == null)
            {
                Assert.Null(userHeader.Tag119_ValidationFlag);
            }
            else
            {
                ValidateTag119(userHeader.Tag119_ValidationFlag, tag119validationFlag);
            }

            if (tag121uniqueEndToEndTransactionReference == null)
            {
                Assert.Null(userHeader.Tag121_UniqueEndToEndTransactionReference);
            }
            else
            {
                ValidateTag121(userHeader.Tag121_UniqueEndToEndTransactionReference, tag121uniqueEndToEndTransactionReference);
            }

            if (tag165paymentReleaseInformationReceiver == null)
            {
                Assert.Null(userHeader.Tag165_PaymentReleaseInformationReceiver);
            }
            else
            {
                ValidateTag165(userHeader.Tag165_PaymentReleaseInformationReceiver, tag165paymentReleaseInformationReceiver);
            }

            if (tag423balanceCheckpointDateAndTime == null)
            {
                Assert.Null(userHeader.Tag423_BalanceCheckpointDateAndTime);
            }
            else
            {
                ValidateTag423(userHeader.Tag423_BalanceCheckpointDateAndTime, tag423balanceCheckpointDateAndTime);
            }

            if (tag424relatedReference == null)
            {
                Assert.Null(userHeader.Tag424_RelatedReference);
            }
            else
            {
                ValidateTag424(userHeader.Tag424_RelatedReference, tag424relatedReference);
            }

            if (tag433sanctionsScreeningInformationForTheReceiver == null)
            {
                Assert.Null(userHeader.Tag433_SanctionsScreeningInformationForTheReceiver);
            }
            else
            {
                ValidateTag433(userHeader.Tag433_SanctionsScreeningInformationForTheReceiver, tag433sanctionsScreeningInformationForTheReceiver);
            }

            if (tag434paymentControlsInformationForReceiver == null)
            {
                Assert.Null(userHeader.Tag434_PaymentControlsInformationForReceiver);
            }
            else
            {
                ValidateTag434(userHeader.Tag434_PaymentControlsInformationForReceiver, tag434paymentControlsInformationForReceiver);
            }
        }
        private static void ValidateTag103(UserHeader.Tag103ServiceIdentifier tag103, string serviceIdentifier)
        {
            Assert.NotNull(tag103);
            CommonBlockDelimitersUnitTest.ValidateCommonBlockDelimiters(tag103.CommonTagDelimiters, "103");
            Assert.Equal(serviceIdentifier, tag103.ServiceIdentifier);
        }

        private static void ValidateTag113(UserHeader.Tag113BankingPriority tag113, string bankingPriority)
        {
            Assert.NotNull(tag113);
            CommonBlockDelimitersUnitTest.ValidateCommonBlockDelimiters(tag113.CommonTagDelimiters, "113");
            Assert.Equal(bankingPriority, tag113.BankingPriority);
        }

        private static void ValidateTag108(UserHeader.Tag108MessageUserReference tag108, string messageUserReference)
        {
            Assert.NotNull(tag108);
            CommonBlockDelimitersUnitTest.ValidateCommonBlockDelimiters(tag108.CommonTagDelimiters, "108");
            Assert.Equal(messageUserReference, tag108.MessageUserReference);
        }

        private static void ValidateTag119(UserHeader.Tag119ValidationFlag tag119, string validationFlag)
        {
            Assert.NotNull(tag119);
            CommonBlockDelimitersUnitTest.ValidateCommonBlockDelimiters(tag119.CommonTagDelimiters, "119");
            Assert.Equal(validationFlag, tag119.ValidationFlag);
        }

        private static void ValidateTag423(UserHeader.Tag423BalanceCheckpointDateAndTime tag423, string balanceCheckpointDateAndTime)
        {
            Assert.NotNull(tag423);
            CommonBlockDelimitersUnitTest.ValidateCommonBlockDelimiters(tag423.CommonTagDelimiters, "423");
            Assert.Equal(balanceCheckpointDateAndTime, tag423.BalanceCheckpointDateAndTime);
        }

        private static void ValidateTag106(UserHeader.Tag106MessageInputReference tag106, string messageInputReference)
        {
            Assert.NotNull(tag106);
            CommonBlockDelimitersUnitTest.ValidateCommonBlockDelimiters(tag106.CommonTagDelimiters, "106");
            Assert.Equal(messageInputReference, tag106.MessageInputReference);
        }

        private static void ValidateTag424(UserHeader.Tag424RelatedReference tag424, string relatedReference)
        {
            Assert.NotNull(tag424);
            CommonBlockDelimitersUnitTest.ValidateCommonBlockDelimiters(tag424.CommonTagDelimiters, "424");
            Assert.Equal(relatedReference, tag424.RelatedReference);
        }

        private static void ValidateTag111(UserHeader.Tag111ServiceTypeIdentifier tag111, string serviceTypeIdentifier)
        {
            Assert.NotNull(tag111);
            CommonBlockDelimitersUnitTest.ValidateCommonBlockDelimiters(tag111.CommonTagDelimiters, "111");
            Assert.Equal(serviceTypeIdentifier, tag111.ServiceTypeIdentifier);
        }

        private static void ValidateTag121(UserHeader.Tag121UniqueEndToEndTransactionReference tag121, string uniqueEndToEndTransactionReference)
        {
            Assert.NotNull(tag121);
            CommonBlockDelimitersUnitTest.ValidateCommonBlockDelimiters(tag121.CommonTagDelimiters, "121");
            Assert.Equal(uniqueEndToEndTransactionReference, tag121.UniqueEndToEndTransactionReference);
        }

        private static void ValidateTag115(UserHeader.Tag115AddresseeInformation tag115, string addresseeInformation)
        {
            Assert.NotNull(tag115);
            CommonBlockDelimitersUnitTest.ValidateCommonBlockDelimiters(tag115.CommonTagDelimiters, "115");
            Assert.Equal(addresseeInformation, tag115.AddresseeInformation);
        }

        private static void ValidateTag165(UserHeader.Tag165PaymentReleaseInformationReceiver tag165, string paymentReleaseInformationReceiver)
        {
            Assert.NotNull(tag165);
            CommonBlockDelimitersUnitTest.ValidateCommonBlockDelimiters(tag165.CommonTagDelimiters, "165");
            Assert.Equal(paymentReleaseInformationReceiver, tag165.PaymentReleaseInformationReceiver);
        }

        private static void ValidateTag433(UserHeader.Tag433SanctionsScreeningInformationForTheReceiver tag433, string sanctionsScreeningInformationForTheReceiver)
        {
            Assert.NotNull(tag433);
            CommonBlockDelimitersUnitTest.ValidateCommonBlockDelimiters(tag433.CommonTagDelimiters, "433");
            Assert.Equal(sanctionsScreeningInformationForTheReceiver, tag433.SanctionsScreeningInformationForTheReceiver);
        }

        private static void ValidateTag434(UserHeader.Tag434PaymentControlsInformationForReceiver tag434, string paymentControlsInformationForReceiver)
        {
            Assert.NotNull(tag434);
            CommonBlockDelimitersUnitTest.ValidateCommonBlockDelimiters(tag434.CommonTagDelimiters, "434");
            Assert.Equal(paymentControlsInformationForReceiver, tag434.PaymentControlsInformationForReceiver);
        }
    }
}
