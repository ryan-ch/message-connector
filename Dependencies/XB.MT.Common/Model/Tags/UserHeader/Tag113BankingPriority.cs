using XB.MT.Common.Model.Common;

namespace XB.MT.Common.Model.Tags.UserHeader
{
    public class Tag113BankingPriority : TagHeader
    {
        public Tag113BankingPriority(CommonBlockDelimiters commonTagDelimiters, string tagValue) : base(commonTagDelimiters)
        {
            BankingPriority = tagValue;
        }

        public string BankingPriority { get; set; }
    }
}
