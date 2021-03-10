using XB.MtParser.Swift_Message;
using Xunit;

namespace XB.MtParser.Tests
{
    public class BasicHeaderTests
    {
        [Theory]
        [InlineData("F", "01", "ESSESES0AXXX", "8000", "025977")]
        [InlineData("F", "01", "BANKFRPPAABC", "4321", "123456")]
        public void BasicHeader_ShouldInitializeWithCorrectValues_WithValidBasicHeaderContent(string appId, string serviceId, string ltAddress, string sessionNumber, string sequenceNumber)
        {
            //Arrange
            var basicHeaderContent = $"{appId}{serviceId}{ltAddress}{sessionNumber}{sequenceNumber}";

            //Act
            var basicHeader = new BasicHeader(basicHeaderContent);

            //Assert
            Assert.Equal(appId, basicHeader.AppID);
            Assert.Equal(serviceId, basicHeader.ServiceID);
            Assert.Equal(ltAddress, basicHeader.LTAddress);
            Assert.Equal(sessionNumber, basicHeader.SessionNumber);
            Assert.Equal(sequenceNumber, basicHeader.SequenceNumber);
        }
    }
}
