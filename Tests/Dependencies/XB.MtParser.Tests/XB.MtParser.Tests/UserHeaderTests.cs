using XB.MtParser.Swift_Message;
using Xunit;

namespace XB.MtParser.Tests
{
    public class UserHeaderTests
    {
        [Theory]
        [InlineData("EBA", "REF0140862562/015", "FIN", "STP", "E01EBC0C-0B22-322Z-A8F1-097839E991F4")]
        [InlineData("XTD", "78", "BTC", "REMIT", "180f1e65-90e0-44d5-a49a-92b55eb3025f")]
        public void UserHeader_ShouldInitializeWithCorrectValues_WithValidUserHeaderContent(string serviceIdentifier, string messageUserReference, string serviceTypeIdentifier, string validationFlag, string uniqueE2EReference)
        {
            //Arrange
            var userHeaderContent = $"{{103:{serviceIdentifier}}}{{108:{messageUserReference}}}{{111:{serviceTypeIdentifier}}}{{119:{validationFlag}}}{{121:{uniqueE2EReference}}}";

            //Act
            var userHeader = new UserHeader(userHeaderContent);

            //Assert
            Assert.Equal(serviceIdentifier, userHeader.ServiceIdentifier);
            Assert.Equal(messageUserReference, userHeader.MessageUserReference);
            Assert.Equal(serviceTypeIdentifier, userHeader.ServiceTypeIdentifier);
            Assert.Equal(validationFlag, userHeader.ValidationFlag);
            Assert.Equal(uniqueE2EReference, userHeader.UniqueEndToEndTransactionReference);
        }
    }
}
