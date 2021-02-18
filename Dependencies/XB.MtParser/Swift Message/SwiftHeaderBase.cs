using System;
using XB.MtParser.Enums;

namespace XB.MtParser.Swift_Message
{
    public abstract record SwiftHeaderBase
    {
        public string HeaderContent { get; init; }

        protected SwiftHeaderBase(string blockContent, SwiftMessageBlockIdentifiers headerType)
        {
            if (string.IsNullOrWhiteSpace(blockContent))
                throw new Exception($"{headerType:G} is empty");

            HeaderContent = blockContent;
        }
    }
}
