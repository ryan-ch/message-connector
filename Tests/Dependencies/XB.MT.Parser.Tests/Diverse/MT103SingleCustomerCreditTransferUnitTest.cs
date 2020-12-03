using System;
using System.Collections.Generic;
using System.Text;
using XB.MT.Parser.Model.Text.MT103;
using XB.MT.Parser.Model.Text.MT103.Fields;
using Xunit;
using static XB.MT.Parser.Model.Text.MT103.MT103SingleCustomerCreditTransferText;

namespace MT103XUnitTestProject.Diverse
{
    public class MT103SingleCustomerCreditTransferUnitTest
    {


        [Fact]
        public void Testa()
        {
            Field71A xyz = new Field71A(new XB.MT.Parser.Model.Common.CommonFieldDelimiters("20"), "lkajsld");
            xyz.DetailsOfCharges = "lkajsld";
            Console.WriteLine(xyz);
        }
    }
}
