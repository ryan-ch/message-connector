using XB.MT.Common.Model.Common;

namespace XB.MT.Common.Model.Tags.UserHeader
{
    public class Tag103ServiceIdentifier : TagHeader
    {
        public string ServiceIdentifier { get; set; }
        public Tag103ServiceIdentifier(CommonBlockDelimiters commonTagDelimiters, string tagValue) : base(commonTagDelimiters)
        {
            ServiceIdentifier = tagValue;
        }
    }
}
