using System;
using System.Collections.Generic;
using System.Text;
using MT103XUnitTestProject.Common;
using XB.MT.Parser.Model.MessageHeader;
using Xunit;

namespace MT103XUnitTestProject.MessageHeader
{
    class BasicHeaderUnitTest
    {
        internal static void ValidateBasicHeader(BasicHeader basicHeader, string appID, string serviceID, string ltAdress,
                                         string sessionNumber, string sequenceNumber)
        {
            Assert.NotNull(basicHeader);
            CommonBlockDelimitersUnitTest.ValidateCommonBlockDelimiters(basicHeader.CommonBlockDelimiters, "1");
            Assert.Equal(appID, basicHeader.AppID);
            Assert.Equal(serviceID, basicHeader.ServiceID);
            Assert.Equal(ltAdress, basicHeader.LTAddress);
            Assert.Equal(sessionNumber, basicHeader.SessionNumber);
            Assert.Equal(sequenceNumber, basicHeader.SequenceNumber);
        }
    }
}
