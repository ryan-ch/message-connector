using System;
using System.Collections.Generic;
using System.Text;

namespace XB.MT.Parser.Model.Common
{
    public class CommonBlockDelimiters
    {
        public CommonBlockDelimiters()
        {
        }

        public CommonBlockDelimiters(string tagId)
        {
            SetDefaultStartOfBlockValues(tagId);
        }

        public string StartOfBlockDelimiter { get; set; }
        public string BlockIdentifier { get; set; }
        public string Separator { get; set; }
        public string EndOfBlockDelimiter { get; set; }

        public void SetDefaultStartOfBlockValues(string blockIdentifier)
        {
            SetDefaultValue(CommonBlockDelimiter.StartOfBlockDelimiter);
            BlockIdentifier = blockIdentifier;
            SetDefaultValue(CommonBlockDelimiter.Separator);
        }

        public void SetDefaultValue(CommonBlockDelimiter commonBlockDelimiter)
        {
            switch (commonBlockDelimiter)
            {
                case CommonBlockDelimiter.StartOfBlockDelimiter:
                    StartOfBlockDelimiter = "{";
                    break;
                case CommonBlockDelimiter.Separator:
                    Separator = ":";
                    break;
                case CommonBlockDelimiter.EndOfBlockDelimiter:
                    EndOfBlockDelimiter = "}";
                    break;
                default:
                    throw new Exception("Not handled CommonBlockDelimiter: " + commonBlockDelimiter);
            }
        }
    }

    public enum CommonBlockDelimiter
    {
        StartOfBlockDelimiter,
        Separator,
        EndOfBlockDelimiter
    }
}
