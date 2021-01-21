using System;

namespace XB.MT.Parser.Model.Common
{
    public class Field
    {
        public CommonFieldDelimiters CommonFieldDelimiters { get; set; }

        public Field(CommonFieldDelimiters commonFieldDelimiters)
        {
            CommonFieldDelimiters = commonFieldDelimiters;
        }

        internal string[] SplitFieldByCrLf(string fieldValue)
        {
            return SplitField(fieldValue, new string[] { Constants.CrLf });
        }
        internal string[] SplitField(string fieldValue, string[] splitValues)
        {
            if (fieldValue != null)
            {
                return fieldValue.Split(splitValues, StringSplitOptions.None);
            }
            else
            {
                return null;
            }
        }
    }
}
