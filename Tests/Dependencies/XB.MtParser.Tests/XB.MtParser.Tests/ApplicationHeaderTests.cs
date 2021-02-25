using System;
using XB.MtParser.Enums;
using XB.MtParser.Swift_Message;
using Xunit;

namespace XB.MtParser.Tests
{
    public class ApplicationHeaderTests
    {
        [Theory]
        [InlineData('I', "103", "SOGEFRPPZXXX", "U", "3", "003")]        
        public void ApplicationHeader_ShouldInitializeWithCorrectValues_WithInputApplicationHeaderContent(char ioIdentifier, string swiftMessageType, string destinationAddress, string priority, string deliveryMonitoring, string obsolescencePeriod)
        {
            //Arrange
            var applicationHeaderContent = $"{ioIdentifier}{swiftMessageType}{destinationAddress}{priority}{deliveryMonitoring}{obsolescencePeriod}";

            //Act
            var applicationHeader = new ApplicationHeader(applicationHeaderContent);

            //Assert
            Assert.Equal(ioIdentifier, applicationHeader.InputOutputIdentifier);
            Assert.Equal(Convert.ToInt32(swiftMessageType), (int)applicationHeader.SwiftMessageType);
            Assert.Equal(destinationAddress, applicationHeader.DestinationAddress);
            Assert.Equal(priority, applicationHeader.Priority);
            Assert.Equal(deliveryMonitoring, applicationHeader.DeliveryMonitoring);
            Assert.Equal(obsolescencePeriod, applicationHeader.ObsolescencePeriod);
            //Output specific properties
            Assert.Null(applicationHeader.InputTime);
            Assert.Null(applicationHeader.MessageInputReference);
            Assert.Null(applicationHeader.OutputDate);
            Assert.Null(applicationHeader.OutputTime);
            Assert.Equal(string.Empty, applicationHeader.LTAddress);
            Assert.Equal(string.Empty, applicationHeader.ISOCountryCode);
        }

        [Theory]
        [InlineData('O', "103", "0955", "100518IRVTUS3NAXXX7676396079", "210215", "1814", "N")]
        public void ApplicationHeader_ShouldInitializeWithCorrectValues_WithOutputApplicationHeaderContent(char ioIdentifier, string swiftMessageType, string inputTime, string messageInputReference, string outputDate, string outputTime, string priority)
        {
            //Arrange
            var applicationHeaderContent = $"{ioIdentifier}{swiftMessageType}{inputTime}{messageInputReference}{outputDate}{outputTime}{priority}";

            //Act
            var applicationHeader = new ApplicationHeader(applicationHeaderContent);

            //Assert
            Assert.Equal(ioIdentifier, applicationHeader.InputOutputIdentifier);
            Assert.Equal(Convert.ToInt32(swiftMessageType), (int)applicationHeader.SwiftMessageType);
            Assert.Equal(inputTime, applicationHeader.InputTime);
            Assert.Equal(messageInputReference, applicationHeader.MessageInputReference);
            Assert.Equal(outputDate, applicationHeader.OutputDate);
            Assert.Equal(outputTime, applicationHeader.OutputTime);
            Assert.Equal(priority, applicationHeader.Priority);
            Assert.Equal(messageInputReference.Substring(6, 12), applicationHeader.LTAddress);
            Assert.Equal(messageInputReference.Substring(10, 2), applicationHeader.ISOCountryCode);
            //Input specific properties
            Assert.Null(applicationHeader.DestinationAddress);
            Assert.Null(applicationHeader.DeliveryMonitoring);
            Assert.Null(applicationHeader.ObsolescencePeriod);
        }

        [Fact]
        public void ApplicationHeader_SwiftMessageShouldBeUnknown_WhenParsingUnacceptedMessageTypes()
        {
            //Arrange
            const string unacceptedMessageTypeApplicationHeaderContent = "I105SOGEFRPPZXXXU3003";

            //Act
            var applicationHeader = new ApplicationHeader(unacceptedMessageTypeApplicationHeaderContent);

            //Assert
            Assert.Equal(SwiftMessageTypes.Unknown, applicationHeader.SwiftMessageType);
        }
    }
}
