using XB.MT.Common.Model.Common;

namespace XB.MT.Common.Model.Tags.UserHeader
{
    public class Tag424RelatedReference : TagHeader
    {
        public Tag424RelatedReference(CommonBlockDelimiters commonTagDelimiters, string tagValue) : base(commonTagDelimiters)
        {
            RelatedReference = tagValue;
        }

        public string RelatedReference { get; set; }
    }
}
