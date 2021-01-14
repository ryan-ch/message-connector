using MT103XUnitTestProject.Common;
using XB.MT.Parser.Model.MessageHeader;
using Xunit;

namespace MT103XUnitTestProject.MessageHeader
{
    internal class TrailerUnitTest
    {
        internal static void ValidateTrailer(Trailer trailer, string tagChecksum, string tagDelayedMessage, string tagMessageReference,
                                     string tagPossibleDuplicateEmission, string tagPossibleDuplicateMessage,
                                     string tagSystemOriginatedMessage, string tagTestAndTrainingMessage)
        {
            Assert.NotNull(trailer);
            CommonBlockDelimitersUnitTest.ValidateCommonBlockDelimiters(trailer.CommonBlockDelimiters, "5");

            if (tagChecksum == null)
            {
                Assert.Null(trailer.Tag_Checksum);
            }
            else
            {
                ValidateTagCHK(trailer.Tag_Checksum, tagChecksum);
            }

            if (tagDelayedMessage == null)
            {
                Assert.Null(trailer.Tag_DelayedMessage);
            }
            else
            {
                ValidateTagDLM(trailer.Tag_DelayedMessage);
            }

            if (tagMessageReference == null)
            {
                Assert.Null(trailer.Tag_MessageReference);
            }
            else
            {
                ValidateTagMRF(trailer.Tag_MessageReference, tagMessageReference);
            }

            if (tagPossibleDuplicateEmission == null)
            {
                Assert.Null(trailer.Tag_PossibleDuplicateEmission);
            }
            else
            {
                ValidateTagPDE(trailer.Tag_PossibleDuplicateEmission, tagPossibleDuplicateEmission);
            }

            if (tagPossibleDuplicateMessage == null)
            {
                Assert.Null(trailer.Tag_PossibleDuplicateMessage);
            }
            else
            {
                ValidateTagPDM(trailer.Tag_PossibleDuplicateMessage, tagPossibleDuplicateMessage);
            }

            if (tagSystemOriginatedMessage == null)
            {
                Assert.Null(trailer.Tag_SystemOriginatedMessage);
            }
            else
            {
                ValidateTagSYS(trailer.Tag_SystemOriginatedMessage, tagSystemOriginatedMessage);
            }

            if (tagTestAndTrainingMessage == null)
            {
                Assert.Null(trailer.Tag_TestAndTrainingMessage);
            }
            else
            {
                ValidateTagTNG(trailer.Tag_TestAndTrainingMessage);
            }
        }

        private static void ValidateTagCHK(Trailer.TagChecksum tagCHK, string checksum)
        {
            Assert.NotNull(tagCHK);
            CommonBlockDelimitersUnitTest.ValidateCommonBlockDelimiters(tagCHK.CommonTagDelimiters, "CHK");
            Assert.Equal(checksum, tagCHK.Checksum);
        }

        private static void ValidateTagTNG(Trailer.TagTestAndTrainingMessage tagTNG)
        {
            Assert.NotNull(tagTNG);
            CommonBlockDelimitersUnitTest.ValidateCommonBlockDelimiters(tagTNG.CommonTagDelimiters, "TNG");
        }

        private static void ValidateTagPDE(Trailer.TagPossibleDuplicateEmission tagPDE, string possibleDuplicateEmission)
        {
            Assert.NotNull(tagPDE);
            CommonBlockDelimitersUnitTest.ValidateCommonBlockDelimiters(tagPDE.CommonTagDelimiters, "PDE");
            Assert.Equal(possibleDuplicateEmission, tagPDE.PossibleDuplicateEmission);
        }

        private static void ValidateTagDLM(Trailer.TagDelayedMessage tagDLM)
        {
            Assert.NotNull(tagDLM);
            CommonBlockDelimitersUnitTest.ValidateCommonBlockDelimiters(tagDLM.CommonTagDelimiters, "DLM");
        }

        private static void ValidateTagMRF(Trailer.TagMessageReference tagMRF, string messageReference)
        {
            Assert.NotNull(tagMRF);
            CommonBlockDelimitersUnitTest.ValidateCommonBlockDelimiters(tagMRF.CommonTagDelimiters, "MRF");
            Assert.Equal(messageReference, tagMRF.MessageReference);
        }

        private static void ValidateTagPDM(Trailer.TagPossibleDuplicateMessage tagPDM, string possibleDuplicateMessage)
        {
            Assert.NotNull(tagPDM);
            CommonBlockDelimitersUnitTest.ValidateCommonBlockDelimiters(tagPDM.CommonTagDelimiters, "PDM");
            Assert.Equal(possibleDuplicateMessage, tagPDM.PossibleDuplicateMessage);
        }

        private static void ValidateTagSYS(Trailer.TagSystemOriginatedMessage tagSYS, string systemOriginatedMessage)
        {
            Assert.NotNull(tagSYS);
            CommonBlockDelimitersUnitTest.ValidateCommonBlockDelimiters(tagSYS.CommonTagDelimiters, "SYS");
            Assert.Equal(systemOriginatedMessage, tagSYS.SystemOriginatedMessage);
        }
    }
}
