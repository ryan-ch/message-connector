using XB.MT.Common.Model.Common;

namespace XB.MT.Parser.Model.MessageHeader
{
    public class BlockHeader
    {
        public CommonBlockDelimiters CommonBlockDelimiters { get; set; }

        public BlockHeader()
        {
            CommonBlockDelimiters = new CommonBlockDelimiters();
        }
    }
}