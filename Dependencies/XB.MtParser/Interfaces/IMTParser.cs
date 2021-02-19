using XB.MtParser.Mt103;

namespace XB.MtParser.Interfaces
{
    public interface IMTParser
    {
        public Mt103Message ParseSwiftMt103Message(string rawSwiftMessage);
    }
}
