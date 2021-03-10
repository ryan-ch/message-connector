using System;
using XB.MtParser.Enums;
using XB.MtParser.Swift_Message;
using Xunit;

namespace XB.MtParser.Tests
{
    public class SwiftHeaderBaseTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void BasicHeader_ShouldThrow_WhenConstructedWithNullOrWhiteSpaceBasicHeaderContent(string basicHeaderContent)
        {
            //Assert
            Assert.Throws<Exception>(() => { _ = new SwiftHeaderBaseMock(basicHeaderContent, default); });
        }

        private record SwiftHeaderBaseMock : SwiftHeaderBase
        {
            public SwiftHeaderBaseMock(string blockContent, SwiftMessageBlockIdentifiers headerType) : base(blockContent, headerType) {}
        }
    }
}
