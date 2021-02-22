using Microsoft.Extensions.Logging;
using Moq;
using System;
using XB.MtParser.Enums;
using XB.MtParser.Mt103;
using Xunit;

namespace XB.MtParser.Tests
{
    public class Mt103MessageTests
    {
        private readonly Mock<ILogger<MtParser>> _loggerMock;
        private readonly string _mockedRawSwiftMessage = "{1:F01ESSESES0AXXX8000019102}{2:O1030955100518CIBCCATTAXXX76763960792012021544N}{3:{103:}{108:WA SQ9E3P}{119:}{111:001}{121:2e66e52d-5448-4742-875a-c39a844bbdc2}}{4:{0}}";
        private readonly string _textBlockContentTemplate = "\r\n:20:GEcG\r\n:23B:CRED\r\n:32A:{0}\r\n:50F:{1}\r\n:59F:{2}\r\n:71A:SHA\r\n:72:/REC/RETN\r\n-";

        public Mt103MessageTests()
        {
            _loggerMock = new Mock<ILogger<MtParser>>();
        }
        
        [Fact]
        public void Mt103Message_EmptyTextBlockContent_WillLogError()
        {
            // Arrange
            var rawSwiftMessage = _mockedRawSwiftMessage.Replace("{0}", string.Empty);

            // Act
            var swiftMt103Message = new Mt103Message(rawSwiftMessage, _loggerMock.Object);

            // Assert
            _loggerMock.Verify(a => a.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString().Contains("Text block is empty")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
              Times.Once);
        }

        [Fact]
        public void Mt103Message_ValidTextBlockContent_WillExtractFields()
        {
            // Arrange
            var field32a = "200825SEK3500,10";
            var field50f = "/SE2880000832790000012345\r\n1/Vårgårda Kromverk\r\n2/Lilla Korsgatan 3\r\n4/19920914\r\n5/Stockholm\r\n6/47281128569335\r\n7/1234567\r\n8/Additional Info";
            var field59f = "/SE3550000000054910123123\r\n1/BOB BAKER\r\n2/Bernhards Gränd 3, 418 42 Göteborg\r\n2/TRANSVERSAL 93 53 48 INT 70\r\n3/CO/BOGOTA";
            var textBlockContent = string.Format(_textBlockContentTemplate, field32a, field50f, field59f);
            var rawSwiftMessage = _mockedRawSwiftMessage.Replace("{0}", textBlockContent);

            // Act
            var swiftMt103Message = new Mt103Message(rawSwiftMessage, _loggerMock.Object);

            // Assert
            Assert.Equal("GEcG", swiftMt103Message.SenderReference);
            Assert.Equal(OperationTypes.CRED, swiftMt103Message.BankOperationCode);
            Assert.Equal(new DateTime(2020, 8, 25), swiftMt103Message.ValueDate);
            Assert.Equal("SEK", swiftMt103Message.Currency);
            Assert.Equal((decimal)3500.10, swiftMt103Message.SettledAmount);
            Assert.Equal(new OrderingCustomer(string.Empty, field50f, string.Empty), swiftMt103Message.OrderingCustomer);
            Assert.Equal(new BeneficiaryCustomer(string.Empty, string.Empty, field59f), swiftMt103Message.BeneficiaryCustomer);
            Assert.Equal(string.Empty, swiftMt103Message.RemittanceInformation);
            Assert.Equal("/REC/RETN", swiftMt103Message.SenderToReceiverInformation);
        }

        [Theory]
        [InlineData("")]
        [InlineData("200825SEK")]
        public void Mt103Message_InvalidField32A_WillLogError(string field32a)
        {
            // Arrange
            var textBlockContent = string.Format(_textBlockContentTemplate, field32a, string.Empty, string.Empty);
            var rawSwiftMessage = _mockedRawSwiftMessage.Replace("{0}", textBlockContent);

            // Act
            var swiftMt103Message = new Mt103Message(rawSwiftMessage, _loggerMock.Object);

            // Assert
            _loggerMock.Verify(a => a.Log(
               LogLevel.Error,
               It.IsAny<EventId>(),
               It.Is<It.IsAnyType>((v, _) => v.ToString().Contains($"Invalid field 32A with value: {field32a}")),
               null,
               It.IsAny<Func<It.IsAnyType, Exception, string>>()),
             Times.Once);
            Assert.Equal(DateTime.MinValue, swiftMt103Message.ValueDate);
            Assert.Null(swiftMt103Message.Currency);
            Assert.Equal(0, swiftMt103Message.SettledAmount);
        }

        [Fact]
        public void Mt103Message_InvalidField32AValueDate_WillLogError()
        {
            // Arrange
            var field32a = "HiDateSEK1000";
            var textBlockContent = string.Format(_textBlockContentTemplate, field32a, string.Empty, string.Empty);
            var rawSwiftMessage = _mockedRawSwiftMessage.Replace("{0}", textBlockContent);

            // Act
            var swiftMt103Message = new Mt103Message(rawSwiftMessage, _loggerMock.Object);

            // Assert
            _loggerMock.Verify(a => a.Log(
               LogLevel.Error,
               It.IsAny<EventId>(),
               It.Is<It.IsAnyType>((v, _) => v.ToString().Contains($"Couldn't extract Date from field 32A with value: {field32a}")),
               null,
               It.IsAny<Func<It.IsAnyType, Exception, string>>()),
             Times.Once);
            Assert.Equal(DateTime.MinValue, swiftMt103Message.ValueDate);
            Assert.Equal("SEK", swiftMt103Message.Currency);
            Assert.Equal(1000, swiftMt103Message.SettledAmount);
        }

        [Fact]
        public void Mt103Message_InvalidField32ASettledAmount_WillLogError()
        {
            // Arrange
            var field32a = "200825SEKSettledAmount";
            var textBlockContent = string.Format(_textBlockContentTemplate, field32a, string.Empty, string.Empty);
            var rawSwiftMessage = _mockedRawSwiftMessage.Replace("{0}", textBlockContent);

            // Act
            var swiftMt103Message = new Mt103Message(rawSwiftMessage, _loggerMock.Object);

            // Assert
            _loggerMock.Verify(a => a.Log(
               LogLevel.Error,
               It.IsAny<EventId>(),
               It.Is<It.IsAnyType>((v, _) => v.ToString().Contains($"Couldn't extract SettledAmount from field 32A with value: {field32a}")),
               null,
               It.IsAny<Func<It.IsAnyType, Exception, string>>()),
             Times.Once);
            Assert.Equal(new DateTime(2020, 8, 25), swiftMt103Message.ValueDate);
            Assert.Equal("SEK", swiftMt103Message.Currency);
            Assert.Equal(0, swiftMt103Message.SettledAmount);
        }
    }
}
