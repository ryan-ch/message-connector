using System.Collections.Generic;
using XB.MT.Parser.Model.Common;

namespace XB.MT.Parser.Model.Text.MT103.Fields
{
    public class Field50K : Field
    {
        public string OrderingCustomer { get; set; }
        public AccountCrLF AccountCrLf { get; set; }
        public List<NameOrAddressCrLf> NameAndAddressList { get; set; }

        public Field50K(CommonFieldDelimiters commonFieldDelimiters, string fieldValue) : base(commonFieldDelimiters)
        {
            OrderingCustomer = fieldValue;

            string[] parts = SplitFieldByCrLf(fieldValue);
            if (parts != null)
            {
                int i = 0;
                foreach (var part in parts)
                {
                    if (i == 0 && part[0] == '/')
                    {
                        AccountCrLf = new AccountCrLF(part, true);
                    }
                    else
                    {
                        if (NameAndAddressList == null)
                        {
                            NameAndAddressList = new List<NameOrAddressCrLf>();
                        }
                        NameAndAddressList.Add(new NameOrAddressCrLf(part, true));
                    }
                    i++;
                }
            }
        }
    }
}
