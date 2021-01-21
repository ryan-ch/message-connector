using XB.MT.Common.Model.Common;

namespace XB.MT.Common.Model.Tags.UserHeader
{
    public class Tag111ServiceTypeIdentifier : TagHeader
    {
        public Tag111ServiceTypeIdentifier(CommonBlockDelimiters commonTagDelimiters, string tagValue) : base(commonTagDelimiters)
        {
            ServiceTypeIdentifier = tagValue;
        }

        public string ServiceTypeIdentifier { get; set; }
    }
}
