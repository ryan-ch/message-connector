using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using XB.MtParser.Enums;
using XB.MtParser.Models;

namespace XB.MtParser.Swift_Message
{
    public record SwiftMessage
    {
        protected readonly ILogger<MtParser> Logger;

        public SwiftMessage(string rawSwiftMessage, ILogger<MtParser> logger)
        {
            Logger = logger;

            OriginalSwiftMessage = rawSwiftMessage;
            Blocks = ParseSwiftMessageBlocks(OriginalSwiftMessage);

            BasicHeader = new BasicHeader(Blocks.FirstOrDefault(block => block.IdentifierAsInt == (int)SwiftMessageBlockIdentifiers.BasicHeader)?.Content);
            ApplicationHeader = new ApplicationHeader(Blocks.FirstOrDefault(block => block.IdentifierAsInt == (int)SwiftMessageBlockIdentifiers.ApplicationHeader)?.Content);
            UserHeader = new UserHeader(Blocks.FirstOrDefault(block => block.IdentifierAsInt == (int)SwiftMessageBlockIdentifiers.UserHeader)?.Content);

            SwiftMessageType = ApplicationHeader.SwiftMessageType;
        }

        public string OriginalSwiftMessage { get; init; }
        public SwiftMessageTypes SwiftMessageType { get; init; } = SwiftMessageTypes.Unknown;
        public BasicHeader BasicHeader { get; init; }
        public ApplicationHeader ApplicationHeader { get; init; }
        public UserHeader UserHeader { get; init; }
        protected List<Block> Blocks { get; init; }

        private List<Block> ParseSwiftMessageBlocks(string rawSwiftMessage)
        {
            // Todo: enhance the parsing in case we have unexpected header id like 4B or 33

            // Add the standard Swift blocks if they exist
            var blocks = new List<Block>();
            for (int i = 1; i <= 5; i++)
            {
                var blockStringMatch = Regex.Match(rawSwiftMessage, $"{{{i}:(.*?)}}({{\\w:|$)", RegexOptions.Singleline);
                if (blockStringMatch.Success)
                    blocks.Add(new Block(i.ToString(), blockStringMatch.Groups[1].Value));
            }

            // Add the special block if exists
            var specialBlockMatch = Regex.Match(rawSwiftMessage, $"{{S:(.*?)}}$", RegexOptions.Singleline);
            if (specialBlockMatch.Success)
                blocks.Add(new Block(SwiftMessageBlockIdentifiers.Special.ToString("d"), specialBlockMatch.Groups[1].Value));

            return blocks;
        }
    }
}
