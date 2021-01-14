using System;

namespace XB.MT.Parser.Model.Common
{
    public class CommonFieldDelimiters
    {
        public CommonFieldDelimiters(string fieldId)
        {
            SetDefaultStartOfFieldValues(fieldId);
        }

        public string StartOfFieldDelimiter { get; set; }
        public string FieldIdentifier { get; set; }
        public string Separator { get; set; }
        public string CarriageReturn { get; set; }
        public string LineFeed { get; set; }

        public void SetDefaultStartOfFieldValues(string fieldIdentifier)
        {
            SetDefaultValue(CommonFieldDelimiter.StartOfFieldDelimiter);
            FieldIdentifier = fieldIdentifier;
            SetDefaultValue(CommonFieldDelimiter.Separator);
        }

        public void SetCarriageReturnNewLine()
        {
            SetDefaultValue(CommonFieldDelimiter.CarriageReturn);
            SetDefaultValue(CommonFieldDelimiter.LineFeed);
        }

        public void SetDefaultValue(CommonFieldDelimiter commonFieldDelimiter)
        {
            switch (commonFieldDelimiter)
            {
                case CommonFieldDelimiter.StartOfFieldDelimiter:
                    StartOfFieldDelimiter = ":";
                    break;
                case CommonFieldDelimiter.Separator:
                    Separator = ":";
                    break;
                case CommonFieldDelimiter.CarriageReturn:
                    CarriageReturn = "\r";
                    break;
                case CommonFieldDelimiter.LineFeed:
                    LineFeed = "\n";
                    break;
                default:
                    throw new Exception("Not handled CommonFieldDelimiter: " + commonFieldDelimiter);
            }
        }
    }


    public enum CommonFieldDelimiter
    {
        StartOfFieldDelimiter,
        Separator,
        CarriageReturn,
        LineFeed
    }
}
