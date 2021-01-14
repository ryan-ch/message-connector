using System;
using XB.MT.Parser.Model.Text.MT103.Fields;
using Xunit;

namespace MT103XUnitTestProject.Diverse
{
    public class MT103SingleCustomerCreditTransferUnitTest
    {


        [Fact]
        public void Testa()
        {
            Field71A xyz = new Field71A(new XB.MT.Parser.Model.Common.CommonFieldDelimiters("20"), "lkajsld")
            {
                DetailsOfCharges = "lkajsld"
            };
            Console.WriteLine(xyz);
        }
    }
}
