using XB.MT.Common.Model.Common;

namespace XB.MT.Common.Model.Tags
{
    public class TagHeader
    {
        public CommonBlockDelimiters CommonTagDelimiters { get; set; }

        public TagHeader()
        {
            CommonTagDelimiters = new CommonBlockDelimiters();
        }

        public TagHeader(CommonBlockDelimiters commonTagDelimiters)
        {
            CommonTagDelimiters = commonTagDelimiters;
        }
    }
}
