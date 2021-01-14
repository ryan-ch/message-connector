using XB.MT.Parser.Model.Common;

namespace XB.MT.Parser.Model.Text.MT103.Fields
{
    public class Field71F : Field
    {
        public Field71F(CommonFieldDelimiters commonFieldDelimiters, string fieldValue) : base(commonFieldDelimiters)
        {
            SendersCharges = fieldValue;
        }

        public string SendersCharges { get; set; }
    }
}
