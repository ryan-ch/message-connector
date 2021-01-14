namespace XB.MT.Parser.Model.MessageHeader
{
    public class ApplicationHeaderOutputMessage : ApplicationHeader
    {
        public string InputTime { get; set; }
        public string MessageInputReference { get; set; }
        public string OutputDate { get; set; }
        public string OutputTime { get; set; }
    }
}
