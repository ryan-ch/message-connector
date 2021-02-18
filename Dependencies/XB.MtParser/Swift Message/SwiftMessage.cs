using Microsoft.Extensions.Logging;
using System;
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

            BasicHeader = ExtractHeader(BasicHeader.HeaderType) as BasicHeader;
            ApplicationHeader = ExtractHeader(ApplicationHeader.HeaderType) as ApplicationHeader;
            UserHeader = ExtractHeader(UserHeader.HeaderType) as UserHeader;

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
            for (var i = 1; i <= 5; i++)
            {
                try
                {
                    var blockStringMatch = Regex.Match(rawSwiftMessage, $"{{{i}:(.*?)}}({{\\w:|$)", RegexOptions.Singleline);
                    if (blockStringMatch.Success)
                        blocks.Add(new Block(i.ToString(), blockStringMatch.Groups[1].Value));
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, $"Couldn't parse block {i} from swift message: |{rawSwiftMessage}|");
                }
            }

            // Add the special block if exists
            // var specialBlockMatch = Regex.Match(rawSwiftMessage, $"{{S:(.*?)}}$", RegexOptions.Singleline);
            // if (specialBlockMatch.Success)
            //    blocks.Add(new Block(SwiftMessageBlockIdentifiers.Special.ToString("d"), specialBlockMatch.Groups[1].Value));

            return blocks;
        }

        private SwiftHeaderBase ExtractHeader(SwiftMessageBlockIdentifiers headerType)
        {
            var block = Blocks.FirstOrDefault(b => b.IdentifierAsInt == (int)headerType)?.Content;
            try
            {
                return headerType switch
                {
                    SwiftMessageBlockIdentifiers.BasicHeader => new BasicHeader(block),
                    SwiftMessageBlockIdentifiers.ApplicationHeader => new ApplicationHeader(block),
                    SwiftMessageBlockIdentifiers.UserHeader => new UserHeader(block),
                    _ => throw new ArgumentOutOfRangeException(nameof(headerType), headerType, "Unknown header type: " + headerType.ToString("G"))
                };
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Couldn't extract {headerType:G} details from block: |{block}|");
                return null;
            }
        }
    }
}
