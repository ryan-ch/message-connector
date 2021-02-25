using System.Collections.Generic;
using Testing.Common.Test_Data;
using XB.MtParser.Swift_Message;
using Xunit;

namespace XB.MtParser.Tests
{
    public class SwiftMessageTests
    {
        public static IEnumerable<object[]> SwiftMessagesProvider => new[]
        {
            new object[] { SwiftMessages.SwiftMessage_1.OriginalMessage, SwiftMessages.SwiftMessage_1.BasicHeader, SwiftMessages.SwiftMessage_1.ApplicationHeader, SwiftMessages.SwiftMessage_1.UserHeader, SwiftMessages.SwiftMessage_1.TextBlock },
            new object[] { SwiftMessages.SwiftMessage_2.OriginalMessage, SwiftMessages.SwiftMessage_2.BasicHeader, SwiftMessages.SwiftMessage_2.ApplicationHeader, SwiftMessages.SwiftMessage_2.UserHeader, SwiftMessages.SwiftMessage_2.TextBlock },
        };

        [Theory]
        [MemberData(nameof(SwiftMessagesProvider))]
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
        [MemberData(nameof(SwiftMessagesProvider))]
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
