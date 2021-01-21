using XB.MT.Common.Model.Common;

namespace XB.MT.Common.Model.Tags.UserHeader
{
    public class Tag121UniqueEndToEndTransactionReference : TagHeader
    {
        public Tag121UniqueEndToEndTransactionReference(CommonBlockDelimiters commonTagDelimiters, string tagValue) : base(commonTagDelimiters)
        {
            UniqueEndToEndTransactionReference = tagValue;
        }

        public string UniqueEndToEndTransactionReference { get; set; }
    }
}
