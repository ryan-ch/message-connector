using Microsoft.Extensions.Logging;
using Moq;
using System;
using Testing.Common;
using Testing.Common.Test_Data;
using XB.MtParser.Enums;
using XB.MtParser.Mt103;
using Xunit;

namespace XB.MtParser.Tests
{
    public class Mt103MessageTests
    {
        private readonly Mock<ILogger<MtParser>> _loggerMock;

        public Mt103MessageTests()
        {
            _loggerMock = new Mock<ILogger<MtParser>>();
        }

        [Fact]
        public void Mt103Message_EmptyTextBlockContent_WillLogError()
        {
            // Arrange
            var rawSwiftMessage = SwiftMessagesMock.SwiftMessage_2.OriginalMessage.Replace(SwiftMessagesMock.SwiftMessage_2.TextBlock, "");

            // Act
            _ = new Mt103Message(rawSwiftMessage, _loggerMock.Object);

            // Assert
            _loggerMock.VerifyLoggerCall(LogLevel.Error, "Can't parse Mt103 when text block is empty", Times.Once());
        }

        [Fact]
        public void Mt103Message_IfPassedIncorrectMessageType_WillLogError()
        {
            // Arrange
            var rawSwiftMessage = SwiftMessagesMock.SwiftMessage_2.OriginalMessage.Replace(SwiftMessagesMock.SwiftMessage_2.ApplicationHeader, 
                "O1020955100518IRVTUS3NAXXX76763960792102151814N");

            // Act
            _ = new Mt103Message(rawSwiftMessage, _loggerMock.Object);

            // Assert
            _loggerMock.VerifyLoggerCall(LogLevel.Error, "Can't parse the message if type is not Mt103", Times.Once());
        }

        [Fact]
        public void Mt103Message_ValidTextBlockContent_WillExtractFields()
        {
            // Arrange
            const string field32A = "200825SEK3500,10";
            const string field50F = "/SE2880000832790000012345\r\n1/Vårgårda Kromverk\r\n2/Lilla Korsgatan 3\r\n4/19920914\r\n6/BQ/1zWLCaVqFd3Rs/47281128569335\r\n7/AZ/ynai3oTv8DtC91iwYm87b-vXtWBhRG\r\n8/ynai3oTv8DtC91iwYm87b-vXtWBhRG";
            const string field59F = "/SE3550000000054910123123\r\n1/BOB BAKER\r\n2/Bernhards Gränd 3, 418 42 Göteborg\r\n2/TRANSVERSAL 93 53 48 INT 70\r\n3/CO/BOGOTA";
            var rawSwiftMessage = SwiftMessagesMock.SwiftMessage_1.OriginalMessage.Replace("200825SEK3500,00", field32A);

            // Act
            var swiftMt103Message = new Mt103Message(rawSwiftMessage, _loggerMock.Object);

            // Assert
            Assert.Equal("GEcG", swiftMt103Message.SenderReference);
            Assert.Equal(OperationTypes.CRED, swiftMt103Message.BankOperationCode);
            Assert.Equal(new DateTime(2020, 8, 25), swiftMt103Message.ValueDate);
            Assert.Equal("SEK", swiftMt103Message.Currency);
            Assert.Equal((decimal)3500.10, swiftMt103Message.SettledAmount);
            Assert.Equal(new OrderingCustomer(string.Empty, field50F, string.Empty), swiftMt103Message.OrderingCustomer);
            Assert.Equal(new BeneficiaryCustomer(string.Empty, string.Empty, field59F), swiftMt103Message.BeneficiaryCustomer);
            Assert.Equal(string.Empty, swiftMt103Message.RemittanceInformation);
            Assert.Equal("/REC/RETN", swiftMt103Message.SenderToReceiverInformation);
        }

        [Theory]
        [InlineData("")]
        [InlineData("200825SEK")]
        public void Mt103Message_InvalidField32A_WillLogError(string field32A)
        {
            // Arrange
            var rawSwiftMessage = SwiftMessagesMock.SwiftMessage_2.OriginalMessage.Replace(SwiftMessagesMock.SwiftMessage_2.TextBlock, $":32A:{field32A}\r\n:50F:");

            // Act
            var swiftMt103Message = new Mt103Message(rawSwiftMessage, _loggerMock.Object);

            // Assert
            _loggerMock.VerifyLoggerCall(LogLevel.Error, $"Invalid field 32A with value: {field32A}", Times.Once());

            Assert.Equal(DateTime.MinValue, swiftMt103Message.ValueDate);
            Assert.Null(swiftMt103Message.Currency);
            Assert.Equal(0, swiftMt103Message.SettledAmount);
        }

        [Fact]
        public void Mt103Message_InvalidField32AValueDate_WillLogError()
        {
            // Arrange
            const string field32A = "HiDateSEK1000";
            var rawSwiftMessage = SwiftMessagesMock.SwiftMessage_2.OriginalMessage.Replace(SwiftMessagesMock.SwiftMessage_2.TextBlock, $":32A:{field32A}\r\n:50F:");

            // Act
            var swiftMt103Message = new Mt103Message(rawSwiftMessage, _loggerMock.Object);

            // Assert
            _loggerMock.VerifyLoggerCall(LogLevel.Error, $"Couldn't extract Date from field 32A with value: {field32A}", Times.Once());

            Assert.Equal(DateTime.MinValue, swiftMt103Message.ValueDate);
            Assert.Equal("SEK", swiftMt103Message.Currency);
            Assert.Equal(1000, swiftMt103Message.SettledAmount);
        }

        [Fact]
        public void Mt103Message_InvalidField32ASettledAmount_WillLogError()
        {
            // Arrange
            const string field32A = "200825SEKSettledAmount";
            var rawSwiftMessage = SwiftMessagesMock.SwiftMessage_2.OriginalMessage.Replace(SwiftMessagesMock.SwiftMessage_2.TextBlock, $":32A:{field32A}\r\n:50F:");

            // Act
            var swiftMt103Message = new Mt103Message(rawSwiftMessage, _loggerMock.Object);

            // Assert
            _loggerMock.VerifyLoggerCall(LogLevel.Error, $"Couldn't extract SettledAmount from field 32A with value: {field32A}", Times.Once());

            Assert.Equal(new DateTime(2020, 8, 25), swiftMt103Message.ValueDate);
            Assert.Equal("SEK", swiftMt103Message.Currency);
            Assert.Equal(0, swiftMt103Message.SettledAmount);
        }
    }
}
