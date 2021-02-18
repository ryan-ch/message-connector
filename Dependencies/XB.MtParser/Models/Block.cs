using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace XB.MtParser.Models
{
    public record Block
    {
        public const char StartOfBlockIndicator = '{';
        public const char EndOfBlockIndicator = '}';
        public const char Separator = ':';

        public Block(string identifier, string content)
        {
            Identifier = identifier;
            Content = content;
        }

        public string Identifier { get; init; }
        public int IdentifierAsInt => int.TryParse(Identifier, out var result) ? result : -1;
        public string Content { get; init; }
        
        public static List<Block> ParseOneLevelBlocks(string blocksString)
        {
            var blockRegex = Regex.Matches(blocksString, "{(\\w{1,3}):(.*?)}", RegexOptions.Singleline);
            return blockRegex.Where(a => a.Success).Select(a => new Block(a.Groups[1].Value, a.Groups[2].Value)).ToList();
        }

        public override string ToString()
        {
            return $"{StartOfBlockIndicator}{Identifier}{Separator}{Content}{EndOfBlockIndicator}";
        }
    }
}
