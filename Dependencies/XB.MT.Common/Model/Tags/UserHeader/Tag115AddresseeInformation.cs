using XB.MT.Common.Model.Common;

namespace XB.MT.Common.Model.Tags.UserHeader
{
    public class Tag115AddresseeInformation : TagHeader
    {
        public Tag115AddresseeInformation(CommonBlockDelimiters commonTagDelimiters, string tagValue) : base(commonTagDelimiters)
        {
            AddresseeInformation = tagValue;
        }

        public string AddresseeInformation { get; set; }
    }
}
