using System;
using System.Collections.Generic;
using System.Text;
using XB.MT.Parser.Model.Common;

namespace XB.MT.Parser.Model.Text.MT103.Fields
{
    public class Field71A : Field
    {
        public Field71A(CommonFieldDelimiters commonFieldDelimiters, string fieldValue) : base(commonFieldDelimiters)
        {
            DetailsOfCharges = fieldValue;
        }

        public string DetailsOfCharges { get; set; }
    }
}
