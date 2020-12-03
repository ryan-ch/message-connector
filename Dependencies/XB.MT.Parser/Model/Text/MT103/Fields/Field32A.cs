using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using XB.MT.Parser.Model.Common;

namespace XB.MT.Parser.Model.Text.MT103.Fields
{
    public class Field32A : Field
    {
        public Field32A(CommonFieldDelimiters commonFieldDelimiters, string fieldValue) : base(commonFieldDelimiters)
        {
            ValueDate_Currency_InterbankSettledAmount = fieldValue;
            ValueDate = DateTime.ParseExact(ValueDate_Currency_InterbankSettledAmount.Substring(0, 6),
                "yyMMdd", CultureInfo.InvariantCulture);
            Currency = ValueDate_Currency_InterbankSettledAmount.Substring(6, 3);
            InterbankSettledAmount = double.Parse(ValueDate_Currency_InterbankSettledAmount.Substring(9, ValueDate_Currency_InterbankSettledAmount.Length - 9));
        }

        public string ValueDate_Currency_InterbankSettledAmount { get; set; }
        public DateTime ValueDate { get; set; }
        public string Currency { get; set; }
        public double InterbankSettledAmount { get; set; }
    }
}
