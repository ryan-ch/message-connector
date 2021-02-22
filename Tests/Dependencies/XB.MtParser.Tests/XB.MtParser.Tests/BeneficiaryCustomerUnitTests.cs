using XB.MtParser.Mt103;
using Xunit;

namespace XB.MtParser.Tests
{
    public class BeneficiaryCustomerUnitTests
    {
        private const string Field59_1 = @"/50601001079
BENEF";
        private const string Field59A_1 = @"/SE1350000000054910000011
WXYZSESS";
        private const string Field59F_1 = @"/SE3550000000054910123123
1/BOB BAKER
2/Bernhards Gränd 3, 418 42 Göteborg
2/TRANSVERSAL 93 53 48 INT 70
3/CO/BOGOTA";

        [Theory]
        [InlineData(Field59_1, "50601001079", "BENEF", "")]
        public void BeneficiaryCustomer_WillParseField59_correctly(string field59, string account, string name, string address)
        {
            // Arrange

            // Act
            var result = new BeneficiaryCustomer(field59, "", "");

            // Assert
            Assert.Equal(account, result.Account);
            Assert.Equal(name, result.Name);
            Assert.Equal(address, result.Address);
        }

        [Theory]
        [InlineData(Field59A_1, "SE1350000000054910000011", "WXYZSESS")]
        public void BeneficiaryCustomer_WillParseField59A_correctly(string field59A, string account, string identifierCode)
        {
            // Arrange

            // Act
            var result = new BeneficiaryCustomer("", field59A, "");

            // Assert
            Assert.Equal(account, result.Account);
            Assert.Equal(identifierCode, result.IdentifierCode);
        }

        [Theory]
        [InlineData(Field59F_1, "SE3550000000054910123123", "BOB BAKER", "Bernhards Gränd 3, 418 42 Göteborg\r\nTRANSVERSAL 93 53 48 INT 70", "CO/BOGOTA")]
        public void BeneficiaryCustomer_WillParseField59F_correctly(string field59F, string account, string name, string address, string countryAndTown)
        {
            // Arrange

            // Act
            var result = new BeneficiaryCustomer("", "", field59F);

            // Assert
            Assert.Equal(account, result.Account);
            Assert.Equal(name, result.Name);
            Assert.Equal(address, result.Address);
            Assert.Equal(countryAndTown, result.CountryAndTown);
        }
    }
}
