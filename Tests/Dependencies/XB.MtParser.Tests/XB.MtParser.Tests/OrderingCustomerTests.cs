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
            var account = "123456";
            var identifierCode = "test identifier code";
            var field50a = $"{"/"}{account}\r\n{identifierCode}";

            // Act
            var result = new OrderingCustomer(field50a, string.Empty, string.Empty);

            // Assert
            Assert.Equal(account, result.Account);
            Assert.Equal(identifierCode, result.IdentifierCode);
        }

        [Theory]
        [InlineData("","DRLC/BE/BRUSSELS/NB0949042")]
        [InlineData("SE2880000832790000012345", null)]
        public void OrderingCustomer_WillExtractField50F(string account, string partyIdentifier)
        {
            // Arrange
            var firstPlaceHolder = string.IsNullOrEmpty(account) ? partyIdentifier : "/" + account;
            var name = "test name";
            var address = "test address";
            var countryAndTown = "test country and test town";
            var dob = "19920914";
            var placeOfBirth = "test place of birth";
            var customerIdNo = "47281128569335";
            var nationalIdNo = "1234567";
            var addInfo = "test additional info";

            var field50f = $"{firstPlaceHolder}\r\n{"1/"}{name}\r\n{"2/"}{address}\r\n{"3/"}{countryAndTown}\r\n" +
                $"{"4/"}{dob}\r\n{"5/"}{placeOfBirth}\r\n{"6/"}{customerIdNo}\r\n{"7/"}{nationalIdNo}\r\n{"8/"}{addInfo}";

            // Act
            var result = new OrderingCustomer(string.Empty, field50f, string.Empty);

            // Assert
            Assert.Equal(partyIdentifier, result.PartyIdentifier);
            Assert.Equal(account, result.Account);
            Assert.Equal(name, result.Name);
            Assert.Equal(address, result.Address);
            Assert.Equal(countryAndTown, result.CountryAndTown);
            Assert.Equal(new DateTime(1992,9,14), result.DateOfBirth);
            Assert.Equal(placeOfBirth, result.PlaceOfBirth);
            Assert.Equal(customerIdNo, result.CustomerIdentificationNumber);
            Assert.Equal(nationalIdNo, result.NationalIdentityNumber);
            Assert.Equal(addInfo, result.AdditionalInformation);
        }

        [Fact]
        public void OrderingCustomer_WillExtractField50K()
        {
            // Arrange
            var account = "123456";
            var name = "test name";
            var address = "test address";
            var field50k = $"{"/"}{account}\r\n{name}\r\n{address}";

            // Act
            var result = new OrderingCustomer(string.Empty, string.Empty, field50k);

            // Assert
            Assert.Equal(account, result.Account);
            Assert.Equal(name, result.Name);
            Assert.Equal(address, result.Address);
        }
    }
}
