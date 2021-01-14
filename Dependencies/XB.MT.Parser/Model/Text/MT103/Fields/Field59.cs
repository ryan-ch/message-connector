using System.Collections.Generic;
using XB.MT.Parser.Model.Common;

namespace XB.MT.Parser.Model.Text.MT103.Fields
{
    public class Field59 : Field
    {

        public string BeneficiaryCustomer { get; set; }
        public AccountCrLF AccountCrLf { get; set; }
        public List<NameOrAddressCrLf> NameAndAddress { get; set; }

        public Field59(CommonFieldDelimiters commonFieldDelimiters, string fieldValue) : base(commonFieldDelimiters)
        {
            BeneficiaryCustomer = fieldValue;

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
                        if (NameAndAddress == null)
                        {
                            NameAndAddress = new List<NameOrAddressCrLf>();
                        }
                        NameAndAddress.Add(new NameOrAddressCrLf(part, true));
                    }
                    i++;
                }
            }
        }
    }
}
