using XB.MT.Common.Model.Common;

namespace XB.MT.Common.Model.Tags.UserHeader
{
    public class Tag119ValidationFlag : TagHeader
    {
        public Tag119ValidationFlag(CommonBlockDelimiters commonTagDelimiters, string tagValue) : base(commonTagDelimiters)
        {
            ValidationFlag = tagValue;
        }

        public string ValidationFlag { get; set; }
    }
}
