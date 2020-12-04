using System;
using System.Collections.Generic;
using System.Text;
using XB.MT.Parser.Model.Common;

namespace XB.MT.Parser.Model.Text.MT103.Fields
{
    public class Field23B : Field
    {
        public Field23B(CommonFieldDelimiters commonFieldDelimiters, string fieldValue) : base(commonFieldDelimiters)
        {
            BankOperationCode = fieldValue;
        }

        public string BankOperationCode { get; set; }
    }
}
