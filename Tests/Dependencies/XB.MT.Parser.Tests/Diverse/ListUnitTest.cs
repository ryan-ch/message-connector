using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MT103XUnitTestProject.Diverse
{
    public class ListUnitTest
    {
        [Fact]
        public void ListTest()
        {
            List<string> l1 = new List<string>();
            l1.Add("a");
            List<string> l2 = new List<string>();
            l2.Add("a");
            bool lika = l1.Equals(l2);
        }
    }
}
