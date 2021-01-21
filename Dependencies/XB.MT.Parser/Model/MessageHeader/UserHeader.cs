using XB.MT.Common.Model.Tags.UserHeader;

namespace XB.MT.Parser.Model.MessageHeader
{
    public class UserHeader : BlockHeader
    {
        public const string _103Key = "103";
        public const string _113Key = "113";
        public const string _108Key = "108";
        public const string _119Key = "119";
        public const string _423Key = "423";
        public const string _106Key = "106";
        public const string _424Key = "424";
        public const string _111Key = "111";
        public const string _121Key = "121";
        public const string _115Key = "115";
        public const string _165Key = "165";
        public const string _433Key = "433";
        public const string _434Key = "434";


        public Tag103ServiceIdentifier Tag103_ServiceIdentifier { get; set; }
        public Tag113BankingPriority Tag113_BankingPriority { get; set; }
        public Tag108MessageUserReference Tag108_MessageUserReference { get; set; }
        public Tag119ValidationFlag Tag119_ValidationFlag { get; set; }
        public Tag423BalanceCheckpointDateAndTime Tag423_BalanceCheckpointDateAndTime { get; set; }
        public Tag106MessageInputReference Tag106_MessageInputReference { get; set; }
        public Tag424RelatedReference Tag424_RelatedReference { get; set; }
        public Tag111ServiceTypeIdentifier Tag111_ServiceTypeIdentifier { get; set; }
        public Tag121UniqueEndToEndTransactionReference Tag121_UniqueEndToEndTransactionReference { get; set; }
        public Tag115AddresseeInformation Tag115_AddresseeInformation { get; set; }
        public Tag165PaymentReleaseInformationReceiver Tag165_PaymentReleaseInformationReceiver { get; set; }
        public Tag433SanctionsScreeningInformationForTheReceiver Tag433_SanctionsScreeningInformationForTheReceiver { get; set; }
        public Tag434PaymentControlsInformationForReceiver Tag434_PaymentControlsInformationForReceiver { get; set; }
    }
}
