using XB.MtParser.Mt103;
using Xunit;

namespace XB.MtParser.Tests
{
    public class ComplexFieldBaseUnitTests
    {
        [Theory]
        [InlineData("/50601001079", "/50601001079")]
        [InlineData("50601001079\r\n", "50601001079")]
        [InlineData("50601001079\r\n0000000", "50601001079")]
        [InlineData("50601001079\n0000000", "50601001079")]
        public void ExtractNextLine_WillExtractNextLineWithoutPrefix_correctly(string originalString, string expectedResult)
        {
            // Arrange
            // Act
            var result = new ComplexFieldBaseMock().ExtractNextLineMock(originalString, "", false, out _);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("/50601001079", "/", "50601001079")]
        [InlineData("50601001079", "/", "")]
        [InlineData("50601001079/0000000\r\n", "/", "")]
        [InlineData("50601001079\r\n/0000000\r\n", "/", "0000000")]
        [InlineData("50601001079\r\n0000000\r\n1/newLine", "1/", "newLine")]
        [InlineData("50601001079\r\n1/0000000\r\n1/newLine\r\n", "1/", "0000000")]
        public void ExtractNextLine_WillExtractNextLineWithPrefix_correctly(string originalString, string prefix, string expectedResult)
        {
            // Arrange
            // Act
            var result = new ComplexFieldBaseMock().ExtractNextLineMock(originalString, prefix, false, out _);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("/50601001079", true, "")]
        [InlineData("/50601001079", false, "/50601001079")]
        [InlineData("50601001079", true, "50601001079")]
        [InlineData("/50601001079\r\n/0000000", true, "/0000000")]
        [InlineData("50601001079\r\n0000000\r\n/newLine", true, "50601001079\r\n0000000")]
        [InlineData("50601001079\r\n/0000000\r\n/newLine", true, "50601001079\r\n\n/newLine")]
        public void ExtractNextLine_WillRemoveResultFromOriginalString_correctly(string originalString, bool removeFromOriginal, string expectedNewString)
        {
            // Arrange
            // Act
            _ = new ComplexFieldBaseMock().ExtractNextLineMock(originalString, "/", removeFromOriginal, out var newString);

            // Assert
            Assert.Equal(expectedNewString, newString);
        }

        [Theory]
        [InlineData("/50601001079", "", "", "")]
        [InlineData("1/Vårgårda Kromverk\r\n2/Lilla Korsgatan 3\r\n4/19920914", "Vårgårda Kromverk", "Lilla Korsgatan 3", "")]
        [InlineData("2/Lilla Korsgatan 3\r\n4/19920914", "", "Lilla Korsgatan 3", "")]
        [InlineData("1/BOB BAKER\r\n2/Bernhards Gränd 3, 418 42 Göteborg\r\n2/TRANSVERSAL 93 53 48 INT 70\r\n3/CO/BOGOTA",
            "BOB BAKER", "Bernhards Gränd 3, 418 42 Göteborg\r\nTRANSVERSAL 93 53 48 INT 70", "CO/BOGOTA")]
        public void ExtractNameAndAddress_WillExtractNameAndAddress_correctly(string input, string expectedName, string expectedAddress, string expectedCountry)
        {
            // Arrange
            // Act
            var (name, address, country) = new ComplexFieldBaseMock().ExtractNameAndAddressMock(input);

            // Assert
            Assert.Equal(expectedName, name);
            Assert.Equal(expectedAddress, address);
            Assert.Equal(expectedCountry, country);
        }

        private record ComplexFieldBaseMock : ComplexFieldBase
        {
            internal string ExtractNextLineMock(string input, string prefix, bool removeFromOriginalString, out string newString)
            {
                return ExtractNextLine(input, prefix, removeFromOriginalString, out newString);
            }

            internal (string name, string address, string countryAndTown) ExtractNameAndAddressMock(string input)
            {
                return ExtractNameAndAddress(input);
            }
        }
    }
}
