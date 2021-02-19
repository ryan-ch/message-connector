using System.Linq;
using System.Text.RegularExpressions;

namespace XB.MtParser.Mt103
{
    public abstract record ComplexFieldBase
    {
        protected string ExtractNextLine(string input, string prefix, bool removeFromOriginalString, out string newString)
        {
            newString = input;
            var regex = Regex.Match(input, $"^{prefix}(.+)$", RegexOptions.Multiline);
            if (!regex.Success)
                return string.Empty;

            if (removeFromOriginalString)
                newString = newString.Replace(regex.Groups[0].Value, "").Trim('\n', '\r');
            return regex.Groups[1].Value.TrimEnd('\n', '\r');
        }

        protected (string name, string address, string countryAndTown) ExtractNameAndAddress(string input)
        {
            // Lines with number 1 are for the name
            var name = CombineMultiRegexLines(input, "^1/(.+)$", RegexOptions.Multiline);
            // Lines with number 2 for the address
            var address = CombineMultiRegexLines(input, "^2/(.+)$", RegexOptions.Multiline);
            // Lines with number 3 for country and town
            var countryAndTown = CombineMultiRegexLines(input, "^3/(.+)$", RegexOptions.Multiline);

            return new(name, address, countryAndTown);
        }

        private string CombineMultiRegexLines(string input, string pattern, RegexOptions options)
        {
            var result = string.Empty;
            var regex = Regex.Matches(input, pattern, options);
            regex.Where(a => a.Success).ToList().ForEach(a => result += a.Groups[1].Value);

            return result.Trim('\n', '\r');
        }
    }
}
