namespace XB.MT.Parser.Model.MessageHeader
{
    public class BasicHeader : BlockHeader
    {
        public const string AppIDKey = "AppID";
        public const string ServiceIDKey = "ServiceID";
        public const string LTAddressKey = "LTAddress";
        public const string SessionNumberKey = "SessionNumber";
        public const string SequenceNumberKey = "SequenceNumber";

        public string AppID { get; set; }
        public string ServiceID { get; set; }
        public string LTAddress { get; set; }
        public string SessionNumber { get; set; }
        public string SequenceNumber { get; set; }
    }
}
