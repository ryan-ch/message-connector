using XB.MT.Parser.Model.Common;

namespace XB.MT.Parser.Model.Text.MT103.Fields
{
    public class Field52A : Field
    {
        public Field52A(CommonFieldDelimiters commonFieldDelimiters, string fieldValue) : base(commonFieldDelimiters)
        {
            OrderingInstitution = fieldValue;
        }

        public string OrderingInstitution { get; set; }
    }
}
