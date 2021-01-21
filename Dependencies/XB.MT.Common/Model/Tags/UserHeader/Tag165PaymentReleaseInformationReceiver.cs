using XB.MT.Common.Model.Common;

namespace XB.MT.Common.Model.Tags.UserHeader
{
    public class Tag165PaymentReleaseInformationReceiver : TagHeader
    {
        public Tag165PaymentReleaseInformationReceiver(CommonBlockDelimiters commonTagDelimiters, string tagValue) : base(commonTagDelimiters)
        {
            PaymentReleaseInformationReceiver = tagValue;
        }

        public string PaymentReleaseInformationReceiver { get; set; }
    }
}
