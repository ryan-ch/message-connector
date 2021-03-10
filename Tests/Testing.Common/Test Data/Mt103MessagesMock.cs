using XB.MtParser.Mt103;

namespace Testing.Common.Test_Data
{
    public record Mt103MessagesMock
    {
        public static readonly Mt103Message Mt103Message_01 = new Mt103Message(SwiftMessagesMock.SwiftMessage_1.OriginalMessage, null);
        public static readonly Mt103Message Mt103Message_02 = new Mt103Message(SwiftMessagesMock.SwiftMessage_2.OriginalMessage, null);
    }
}
