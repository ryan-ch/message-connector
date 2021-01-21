using System;

namespace XB.MT.Common.Model.Common
{
    public class CommonBlockDelimiters
    {
        /// <summary>
        /// First character in a block, will be '{'
        /// </summary>
        public string StartOfBlockDelimiter { get; set; }
        /// <summary>
        /// Defines block contents 
        /// </summary>
        public string BlockIdentifier { get; set; }
        /// <summary>
        /// Indicates the end of the block identifier, will be ':'
        /// </summary>
        public string Separator { get; set; }
        /// <summary>
        /// Last character in a block, will be '}'
        /// </summary>
        public string EndOfBlockDelimiter { get; set; }

        public CommonBlockDelimiters()
        {
        }

        public CommonBlockDelimiters(string tagId)
        {
            SetDefaultStartOfBlockValues(tagId);
        }


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
