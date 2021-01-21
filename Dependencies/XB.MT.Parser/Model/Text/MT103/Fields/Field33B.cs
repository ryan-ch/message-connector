using XB.MT.Parser.Model.Common;

namespace XB.MT.Parser.Model.Text.MT103.Fields
{
    public class Field33B : Field
    {
        public Field33B(CommonFieldDelimiters commonFieldDelimiters, string fieldValue) : base(commonFieldDelimiters)
        {
            Currency_InstructedAmount = fieldValue;
            if (Currency_InstructedAmount.Length > 2)
            {
                Currency = Currency_InstructedAmount.Substring(0, 3);
            }
            if (Currency_InstructedAmount.Length > 3)
            {
                InstructedAmount = double.Parse(Currency_InstructedAmount.Substring(3));
            }
        }

        public string Currency_InstructedAmount { get; set; }
        public string Currency { get; set; }
        public double InstructedAmount { get; set; }
    }
}
