using System.Collections.Generic;
using XB.MT.Parser.Model.Common;

namespace XB.MT.Parser.Model.Text.MT103.Fields
{
    public class Field50F : Field50Base
    {
        public PartyIdentifierCrLf PartyIdentifier { get; set; }
        public List<AdditionalRow> NumberNameAndAdresses { get; set; }

        public Field50F(CommonFieldDelimiters commonFieldDelimiters, string fieldValue) : base(commonFieldDelimiters, fieldValue)
        {
            string[] parts = SplitFieldByCrLf(fieldValue);
            if (parts != null)
            {
                bool firstPart = true;
                foreach (var part in parts)
                {
                    if (firstPart)
                    {
                        PartyIdentifier = new PartyIdentifierCrLf(part);
                        firstPart = false;
                    }
                    else
                    {
                        if (NumberNameAndAdresses == null)
                        {
                            NumberNameAndAdresses = new List<AdditionalRow>();
                        }
                        NumberNameAndAdresses.Add(new AdditionalRow(part, true));
                    }
                }
            }
        }
    }
}
