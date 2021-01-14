using XB.MT.Parser.Model.Common;

namespace XB.MT.Parser.Model.Text.MT103.Fields
{
    public class Field70 : Field
    {
        public Field70(CommonFieldDelimiters commonFieldDelimiters, string fieldValue) : base(commonFieldDelimiters)
        {
            RemittanceInformation = fieldValue;
        }

        public string RemittanceInformation { get; set; }
    }
}
