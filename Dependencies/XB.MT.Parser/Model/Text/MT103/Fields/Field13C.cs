using System;
using System.Collections.Generic;
using System.Text;
using XB.MT.Parser.Model.Common;

namespace XB.MT.Parser.Model.Text.MT103.Fields
{
    public class Field13C : Field
    {
        public Field13C(CommonFieldDelimiters commonFieldDelimiters, string fieldValue) : base(commonFieldDelimiters)
        {
            TimeIndication = fieldValue;
        }

        public string TimeIndication { get; set; }
    }
}
