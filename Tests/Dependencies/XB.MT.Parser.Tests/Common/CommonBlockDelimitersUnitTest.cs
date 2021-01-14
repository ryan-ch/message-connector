using XB.MT.Parser.Model.Common;
using Xunit;

namespace MT103XUnitTestProject.Common
{
    internal class CommonBlockDelimitersUnitTest
    {
        internal static void ValidateCommonBlockDelimiters(CommonBlockDelimiters commonBlockDelimiters, string blockIdentifier)
        {
            Assert.NotNull(commonBlockDelimiters);
            Assert.Equal("{", commonBlockDelimiters.StartOfBlockDelimiter);
            Assert.Equal(blockIdentifier, commonBlockDelimiters.BlockIdentifier);
            Assert.Equal(":", commonBlockDelimiters.Separator);
            Assert.Equal("}", commonBlockDelimiters.EndOfBlockDelimiter);
        }

    }
}
