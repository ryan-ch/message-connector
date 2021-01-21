using XB.MT.Common.Model.Common;

namespace XB.MT.Common.Model.Tags.UserHeader
{
    public class Tag423BalanceCheckpointDateAndTime : TagHeader
    {
        public Tag423BalanceCheckpointDateAndTime(CommonBlockDelimiters commonTagDelimiters, string tagValue) : base(commonTagDelimiters)
        {
            BalanceCheckpointDateAndTime = tagValue;
        }

        public string BalanceCheckpointDateAndTime { get; set; }
    }
}
