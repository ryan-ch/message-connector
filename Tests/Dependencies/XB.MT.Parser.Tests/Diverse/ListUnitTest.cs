using System.Collections.Generic;
using Xunit;

namespace MT103XUnitTestProject.Diverse
{
    public class ListUnitTest
    {
        [Fact]
        public void ListTest()
        {
            List<string> l1 = new List<string>
            {
                "a"
            };
            List<string> l2 = new List<string>
            {
                "a"
            };
            bool lika = l1.Equals(l2);
        }
    }
}
