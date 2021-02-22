using System.Collections.Generic;
using XB.MtParser.Swift_Message;
using Xunit;

namespace XB.MtParser.Tests
{
    public class SwiftMessageTests
    {
        private const string _mockedRawSwiftMessageTemplate = "{{1:{0}}}{{2:{1}}}{{3:{2}}}{{4:{3}}}{{S:{4}}}";

        [Theory]
        [InlineData("F01ESSESES0AXXX8000019102", "O1030955100518CIBCCATTAXXX76763960792012021544N", "{103:}{108:WA SQ9E3P}{119:}{111:001}{121:2e66e52d-5448-4742-875a-c39a844bbdc2}", @":20:GEcG
:23B:CRED
:32A:200825SEK3500,00
:50F:/SE2880000832790000012345
1/Vårgårda Kromverk
2/Lilla Korsgatan 3
4/19920914
6/BQ/1zWLCaVqFd3Rs/47281128569335
7/AZ/ynai3oTv8DtC91iwYm87b-vXtWBhRG
8/ynai3oTv8DtC91iwYm87b-vXtWBhRG
:59F:/SE3550000000054910123123
1/BOB BAKER
2/Bernhards Gränd 3, 418 42 Göteborg
2/TRANSVERSAL 93 53 48 INT 70
3/CO/BOGOTA
:71A:SHA
:72:/REC/RETN
-", "{MAN:UAKOUAK4600}")]
        public void SwiftMessage_ShouldInitializeCorrectly(string basicHeaderContent, string applicationHeaderContent, string userHeaderContent, string textContent, string specialContent)
        {
            //Arrange
            var rawSwiftMessage = string.Format(_mockedRawSwiftMessageTemplate, basicHeaderContent, applicationHeaderContent, userHeaderContent, textContent, specialContent);

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
        [InlineData("F01ESSESES0AXXX8000019102", "O1030955100518CIBCCATTAXXX76763960792012021544N", "{103:}{108:WA SQ9E3P}{119:}{111:001}{121:2e66e52d-5448-4742-875a-c39a844bbdc2}", @":20:GEcG
:23B:CRED
:32A:200825SEK3500,00
:50F:/SE2880000832790000012345
1/Vårgårda Kromverk
2/Lilla Korsgatan 3
4/19920914
6/BQ/1zWLCaVqFd3Rs/47281128569335
7/AZ/ynai3oTv8DtC91iwYm87b-vXtWBhRG
8/ynai3oTv8DtC91iwYm87b-vXtWBhRG
:59F:/SE3550000000054910123123
1/BOB BAKER
2/Bernhards Gränd 3, 418 42 Göteborg
2/TRANSVERSAL 93 53 48 INT 70
3/CO/BOGOTA
:71A:SHA
:72:/REC/RETN
-", "{MAN:UAKOUAK4600}")]
        public void SwiftMessage_ShouldParseBlocksCorrecly(string basicHeaderContent, string applicationHeaderContent, string userHeaderContent, string textContent, string specialContent)
        {
            //Arrange
            var blockList = new List<string>()
            {
                basicHeaderContent,
                applicationHeaderContent,
                userHeaderContent,
                textContent
            };

            var rawSwiftMessage = string.Format(_mockedRawSwiftMessageTemplate, basicHeaderContent, applicationHeaderContent, userHeaderContent, textContent, specialContent);

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
