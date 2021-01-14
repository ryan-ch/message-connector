using XB.MT.Parser.Model.MessageHeader;

namespace XB.MT.Parser.Model
{
    public class MT : IMT
    {
        public BasicHeader BasicHeader { get; set; }
        // Input message to SWIFT, output from bank
        public ApplicationHeaderInputMessage ApplicationHeaderInputMessage { get; set; }
        // Output message from SWIFT, input to bank
        public ApplicationHeaderOutputMessage ApplicationHeaderOutputMessage { get; set; }
        public UserHeader UserHeader { get; set; }
        public Trailer Trailer { get; set; }
    }
}
