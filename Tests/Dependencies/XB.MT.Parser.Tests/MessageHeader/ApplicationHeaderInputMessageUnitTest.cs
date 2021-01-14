using MT103XUnitTestProject.Common;
using XB.MT.Parser.Model.MessageHeader;
using Xunit;

namespace MT103XUnitTestProject.MessageHeader
{
    internal class ApplicationHeaderInputMessageUnitTest
    {
        internal static void ValidateApplicationHeaderInputMessage(ApplicationHeaderInputMessage applicationHeader, string inputOutputID,
                                                           string messageType, string destinationAddress, string priority,
                                                           string deliveryMonitoring, string obsolescencePeriod)
        {
            CommonBlockDelimitersUnitTest.ValidateCommonBlockDelimiters(applicationHeader.CommonBlockDelimiters, "2");
            Assert.Equal(inputOutputID, applicationHeader.InputOutputID);
            Assert.Equal(messageType, applicationHeader.MessageType);
            Assert.Equal(destinationAddress, applicationHeader.DestinationAddress);
            Assert.Equal(priority, applicationHeader.Priority);
            Assert.Equal(deliveryMonitoring, applicationHeader.DeliveryMonitoring);
            Assert.Equal(obsolescencePeriod, applicationHeader.ObsolescencePeriod);
        }
    }
}
