using XB.MT.Parser.Model.MessageHeader;

namespace XB.MT.Parser.Model
{
    public class MT103 : IMT
    {
        public string OriginalSWIFTmessage { get; set; } = null;
        public BasicHeader BasicHeader { get; set; }
        // Input message to SWIFT, output from bank
        public ApplicationHeaderInputMessage ApplicationHeaderInputMessage { get; set; }
        // Output message from SWIFT, input to bank
        public ApplicationHeaderOutputMessage ApplicationHeaderOutputMessage { get; set; }
        public UserHeader UserHeader { get; set; }
        public Trailer Trailer { get; set; }

        public void SetOriginalSWIFTmessageIfNull(string message)
        {
            if (OriginalSWIFTmessage == null)
            {
                OriginalSWIFTmessage = message;
            }
        }

    }
}
