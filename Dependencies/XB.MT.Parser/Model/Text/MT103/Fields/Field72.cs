using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XB.MT.Parser.Model.Common;

namespace XB.MT.Parser.Model.Text.MT103.Fields
{
    public class Field72 : Field
    {
        public Field72(CommonFieldDelimiters commonFieldDelimiters, string fieldValue) : base(commonFieldDelimiters)
        {
            SenderToReceiverInformation = fieldValue;

            int index = 0;
            SplitFieldByCrLf(fieldValue).ToList().ForEach(row =>
            {
                if (index == 0)
                {
                    Row1 = row;
                } else { 
                    AdditionalRows.Add(new AdditionalRow(row, true));
                }

                index++;
            });
        }

        public string SenderToReceiverInformation { get; set; }

        public string Row1 { get; set; }

        public List<AdditionalRow> AdditionalRows { get; set; } = new List<AdditionalRow>();
    }
}
