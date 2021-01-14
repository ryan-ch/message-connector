namespace XB.MT.Parser.Model.Common
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
