using System;
using XB.MtParser.Mt103;
using Xunit;

namespace XB.MtParser.Tests
{
    public class OrderingCustomerTests
    {

        [Fact]
        public void OrderingCustomer_WillExtractField50A()
        {
            // Arrange
            const string account = "123456";
            const string identifierCode = "test identifier code";
            var field50A = $"/{account}\r\n{identifierCode}";

            // Act
            var result = new OrderingCustomer(field50A, string.Empty, string.Empty);

            // Assert
            Assert.Equal(account, result.Account);
            Assert.Equal(identifierCode, result.IdentifierCode);
        }

        [Theory]
        [InlineData(null, "DRLC/BE/BRUSSELS/NB0949042")]
        [InlineData("/SE2880000832790000012345", null)]
        public void OrderingCustomer_WillExtractField50F(string account, string partyIdentifier)
        {
            // Arrange
            const string name = "test name";
            const string address = "test address";
            const string countryAndTown = "test country and test town";
            const string dob = "19920914";
            const string placeOfBirth = "test place of birth";
            const string customerIdNo = "47281128569335";
            const string nationalIdNo = "1234567";
            const string addInfo = "test additional info";

            var field50F = $"{account ?? partyIdentifier}\r\n1/{name}\r\n2/{address}\r\n3/{countryAndTown}\r\n" +
                $"4/{dob}\r\n5/{placeOfBirth}\r\n6/{customerIdNo}\r\n7/{nationalIdNo}\r\n8/{addInfo}";

            // Act
            var result = new OrderingCustomer(string.Empty, field50F, string.Empty);

            // Assert
            Assert.Equal(partyIdentifier, result.PartyIdentifier);
            Assert.Equal(account?.Substring(1) ?? "", result.Account);
            Assert.Equal(name, result.Name);
            Assert.Equal(address, result.Address);
            Assert.Equal(countryAndTown, result.CountryAndTown);
            Assert.Equal(new DateTime(1992, 9, 14), result.DateOfBirth);
            Assert.Equal(placeOfBirth, result.PlaceOfBirth);
            Assert.Equal(customerIdNo, result.CustomerIdentificationNumber);
            Assert.Equal(nationalIdNo, result.NationalIdentityNumber);
            Assert.Equal(addInfo, result.AdditionalInformation);
        }

        [Fact]
        public void OrderingCustomer_WillExtractField50K()
        {
            // Arrange
            const string account = "123456";
            const string name = "test name";
            const string address = "test address";
            var field50K = $"/{account}\r\n{name}\r\n{address}";

            // Act
            var result = new OrderingCustomer(string.Empty, string.Empty, field50K);

            // Assert
            Assert.Equal(account, result.Account);
            Assert.Equal(name, result.Name);
            Assert.Equal(address, result.Address);
        }
    }
}
