using Microsoft.Extensions.Logging;
using XB.MtParser.Interfaces;
using XB.MtParser.Mt103;

namespace XB.MtParser
{
    public class MtParser : IMTParser
    {
        private readonly ILogger<MtParser> _logger;

        public MtParser(ILogger<MtParser> logger)
        {
            _logger = logger;
        }
        public Mt103Message ParseSwiftMt103Message(string rawSwiftMessage)
        {
            return new Mt103Message(rawSwiftMessage, _logger);
        }
    }
}
