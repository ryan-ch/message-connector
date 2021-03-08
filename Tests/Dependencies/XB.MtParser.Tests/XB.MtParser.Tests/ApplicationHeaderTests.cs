using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Globalization;
using Xunit;
using Testing.Common;
using XB.MtParser.Enums;
using XB.MtParser.Swift_Message;


namespace XB.MtParser.Tests
{
    public class ApplicationHeaderTests
    {
        private readonly Mock<ILogger<MtParser>> _loggerMock;

        public ApplicationHeaderTests()
        {
            _loggerMock = new Mock<ILogger<MtParser>>();
        }

        [Theory]
        [InlineData('I', "103", "SOGEFRPPZXXX", "U", "3", "003")]        
        public void ApplicationHeader_ShouldInitializeWithCorrectValues_WithInputApplicationHeaderContent(char ioIdentifier, string swiftMessageType, string destinationAddress, string priority, string deliveryMonitoring, string obsolescencePeriod)
        {
            //Arrange
            var applicationHeaderContent = $"{ioIdentifier}{swiftMessageType}{destinationAddress}{priority}{deliveryMonitoring}{obsolescencePeriod}";

            //Act
            var applicationHeader = new ApplicationHeader(applicationHeaderContent, _loggerMock.Object);

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
            Assert.Equal(DateTime.MinValue, applicationHeader.OutputDate);
            Assert.Equal(string.Empty, applicationHeader.LTAddress);
            Assert.Equal(string.Empty, applicationHeader.ISOCountryCode);
        }

        [Theory]
        [InlineData('O', "103", "0955", "100518IRVTUS3NAXXX7676396079", "210215", "1814", "N")]
        public void ApplicationHeader_ShouldInitializeWithCorrectValues_WithOutputApplicationHeaderContent(char ioIdentifier, string swiftMessageType, string inputTime, string messageInputReference, string outputDate, string outputTime, string priority)
        {
            //Arrange
            var applicationHeaderContent = $"{ioIdentifier}{swiftMessageType}{inputTime}{messageInputReference}{outputDate}{outputTime}{priority}";
            var expectedOutputDate = DateTime.ParseExact($"{outputDate}{outputTime}", "yyMMddHHmm", CultureInfo.InvariantCulture, DateTimeStyles.None);

            //Act
            var applicationHeader = new ApplicationHeader(applicationHeaderContent, _loggerMock.Object);

            //Assert
            Assert.Equal(ioIdentifier, applicationHeader.InputOutputIdentifier);
            Assert.Equal(Convert.ToInt32(swiftMessageType), (int)applicationHeader.SwiftMessageType);
            Assert.Equal(inputTime, applicationHeader.InputTime);
            Assert.Equal(messageInputReference, applicationHeader.MessageInputReference);
            Assert.Equal(expectedOutputDate, applicationHeader.OutputDate);
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
            var applicationHeader = new ApplicationHeader(unacceptedMessageTypeApplicationHeaderContent, _loggerMock.Object);

            //Assert
            Assert.Equal(SwiftMessageTypes.Unknown, applicationHeader.SwiftMessageType);
        }

        [Fact]
        public void ApplicationHeader_ShouldLogError_WithInvalidApplicationHeaderInput()
        {
            //Arrange
            const string invalidOutputDateTimeApplicationHeaderContent = "O1030955100518IRVTUS3NAXXX7676396079210215ABCDN";

            //Act
            var applicationHeader = new ApplicationHeader(invalidOutputDateTimeApplicationHeaderContent, _loggerMock.Object);

            //Assert
            _loggerMock.VerifyLoggerCall(LogLevel.Error, $"ApplicationHeader: Unable to parse output date correctly", Times.Once());
        }
    }
}
