using XB.MT.Parser.Model.Common;

namespace XB.MT.Parser.Model.Text.MT103.Fields
{
    public class CodeCountryCodeIdentifierCrLf
    {
        public string CodeCountryCodeIdentifier { get; set; }
        public string CarriageReturn { get; set; }
        public string LineFeed { get; set; }

        public CodeCountryCodeIdentifierCrLf(string codeCountryCodeIdentifier, bool setCrLf)
        {
            CodeCountryCodeIdentifier = codeCountryCodeIdentifier;
            if (setCrLf)
            {
                CarriageReturn = Constants.Cr;
                LineFeed = Constants.Lf;
            }
        }
        public override bool Equals(object o)
        {
            if (o == null || !this.GetType().Equals(o.GetType()))
            {
                return false;
            }
            else
            {
                CodeCountryCodeIdentifierCrLf other = (CodeCountryCodeIdentifierCrLf)o;
                return (CodeCountryCodeIdentifier == other.CodeCountryCodeIdentifier &&
                        CarriageReturn == other.CarriageReturn &&
                        LineFeed == other.LineFeed);
            }
        }
    }
}