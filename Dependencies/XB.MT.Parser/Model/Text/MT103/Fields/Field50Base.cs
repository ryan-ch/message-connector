using System;
using System.Collections.Generic;
using System.Text;
using XB.MT.Parser.Model.Common;

namespace XB.MT.Parser.Model.Text.MT103.Fields
{
    public class Field50Base : Field
    {
        public string OrderingCustomer { get; set; }
        public Field50Base(CommonFieldDelimiters commonFieldDelimiters, string fieldValue) : base(commonFieldDelimiters)
        {
            OrderingCustomer = fieldValue;
        }

    }
}
