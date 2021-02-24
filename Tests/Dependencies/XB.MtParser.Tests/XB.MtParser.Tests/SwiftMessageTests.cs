using System.Collections.Generic;
using XB.MtParser.Swift_Message;
using XB.MtParser.Tests.Test_Data;
using Xunit;

namespace XB.MtParser.Tests
{
    public class SwiftMessageTests
    {
        [Theory]
        [MemberData(nameof(SwiftMessageTestData.SwiftMessages), MemberType = typeof(SwiftMessageTestData))]
        public void SwiftMessage_ShouldInitializeCorrectly(string rawSwiftMessage, string basicHeaderContent, string applicationHeaderContent, string userHeaderContent, string textContent)
        {
            //Arrange
            //Act
            var swiftMessage = new SwiftMessage(rawSwiftMessage, null);

            //Assert
            var expectedBasicHeader = new BasicHeader(basicHeaderContent);
            var expectedApplicationHeader = new ApplicationHeader(applicationHeaderContent);
            var expectedUserHeader = new UserHeader(userHeaderContent);
            Assert.Equal(rawSwiftMessage, swiftMessage.OriginalSwiftMessage);
            Assert.Equal(expectedBasicHeader, swiftMessage.BasicHeader);
            Assert.Equal(expectedApplicationHeader, swiftMessage.ApplicationHeader);
            Assert.Equal(expectedUserHeader, swiftMessage.UserHeader);
            Assert.Equal(expectedApplicationHeader.SwiftMessageType, swiftMessage.SwiftMessageType);
        }

        [Theory]
        [MemberData(nameof(SwiftMessageTestData.SwiftMessages), MemberType = typeof(SwiftMessageTestData))]
        public void SwiftMessage_ShouldParseBlocksCorrectly(string rawSwiftMessage, string basicHeaderContent, string applicationHeaderContent, string userHeaderContent, string textContent)
        {
            //Arrange
            var blockList = new List<string>
            {
                basicHeaderContent,
                applicationHeaderContent,
                userHeaderContent,
                textContent
            };

            //Act
            var swiftMessage = new SwiftMessage(rawSwiftMessage, null);

            //Assert
            for (var i = 0; i < swiftMessage.Blocks.Count; i++)
            {
                Assert.Equal(blockList[i], swiftMessage.Blocks[i].Content);
            }
        }
    }
}
