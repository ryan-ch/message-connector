using XB.MT.Common.Model.Common;

namespace XB.MT.Common.Model.Tags.UserHeader
{
    public class Tag433SanctionsScreeningInformationForTheReceiver : TagHeader
    {
        public Tag433SanctionsScreeningInformationForTheReceiver(CommonBlockDelimiters commonTagDelimiters, string tagValue) :
                                                                 base(commonTagDelimiters)
        {
            SanctionsScreeningInformationForTheReceiver = tagValue;
        }

        public string SanctionsScreeningInformationForTheReceiver { get; set; }
    }
}
