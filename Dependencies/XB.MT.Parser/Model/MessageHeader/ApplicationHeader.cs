namespace XB.MT.Parser.Model.MessageHeader
{
    public class ApplicationHeader : BlockHeader
    {

        public const string InputOutputIDKey = "InputOutputID";
        public const string MessageTypeKey = "MessageType";
        public const string PriorityKey = "Priority";

        public string InputOutputID { get; set; }
        public string MessageType { get; set; }
        public string Priority { get; set; }
    }
}
