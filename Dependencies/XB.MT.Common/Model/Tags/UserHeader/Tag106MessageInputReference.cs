using XB.MT.Common.Model.Common;

namespace XB.MT.Common.Model.Tags.UserHeader
{
    public class Tag106MessageInputReference : TagHeader
    {
        public Tag106MessageInputReference(CommonBlockDelimiters commonTagDelimiters, string tagValue) : base(commonTagDelimiters)
        {
            MessageInputReference = tagValue;
        }

        public string MessageInputReference { get; set; }
    }
}
