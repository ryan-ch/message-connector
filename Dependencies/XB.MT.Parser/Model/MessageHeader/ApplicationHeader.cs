namespace XB.MT.Parser.Model.MessageHeader
{
    public class ApplicationHeader : BlockHeader
    {
        public string InputOutputID { get; set; }
        public string MessageType { get; set; }
        public string Priority { get; set; }
    }
}
