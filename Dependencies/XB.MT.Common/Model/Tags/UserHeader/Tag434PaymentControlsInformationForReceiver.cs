using XB.MT.Common.Model.Common;

namespace XB.MT.Common.Model.Tags.UserHeader
{
    public class Tag434PaymentControlsInformationForReceiver : TagHeader
    {
        public Tag434PaymentControlsInformationForReceiver(CommonBlockDelimiters commonTagDelimiters, string tagValue) :
                                                           base(commonTagDelimiters)
        {
            PaymentControlsInformationForReceiver = tagValue;
        }

        public string PaymentControlsInformationForReceiver { get; set; }
    }
}
