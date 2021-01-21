using XB.MT.Common.Model.Common;

namespace XB.MT.Common.Model.Tags.UserHeader
{
    public class Tag108MessageUserReference : TagHeader
    {
        public Tag108MessageUserReference(CommonBlockDelimiters commonTagDelimiters, string tagValue) : base(commonTagDelimiters)
        {
            MessageUserReference = tagValue;
        }

        public string MessageUserReference { get; set; }
    }
}
