using System.Collections.Generic;
using XB.MT.Parser.Model.Common;

namespace XB.MT.Parser.Model.Text.MT103.Fields
{
    public class Field59Base : Field
    {

        public string BeneficiaryCustomer { get; set; }
        public AccountCrLf AccountCrLf { get; set; }
        public List<AdditionalRow> AdditionalRows { get; set; }

        public Field59Base(CommonFieldDelimiters commonFieldDelimiters, string fieldValue) : base(commonFieldDelimiters)
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
                        AccountCrLf = new AccountCrLf(part, true);
                    }
                    else
                    {
                        if (AdditionalRows == null)
                        {
                            AdditionalRows = new List<AdditionalRow>();
                        }
                        AdditionalRows.Add(new AdditionalRow(part, true));
                    }
                    i++;
                }
            }
        }
    }
}
