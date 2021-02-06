using XB.MT.Parser.Model.Common;
using XB.MT.Parser.Model.MessageHeader;
using XB.MT.Parser.Model.Text.MT103.Fields;

namespace XB.MT.Parser.Model.Text.MT103
{

    public class MT103SingleCustomerCreditTransferText : BlockHeader
    {
        public const string _13CKey = "13C";
        public const string _20Key = "20";
        public const string _23BKey = "23B";
        public const string _23EKey = "23E";
        public const string _32AKey = "32A";
        public const string _33BKey = "33B";
        public const string _50AKey = "50A";
        public const string _50FKey = "50F";
        public const string _50KKey = "50K";
        public const string _52AKey = "52A";
        public const string _52DKey = "52D";
        public const string _59Key = "59";
        public const string _59AKey = "59A";
        public const string _59FKey = "59F";
        public const string _70Key = "70";
        public const string _71AKey = "71A";
        public const string _71FKey = "71F";
        public const string _72Key = "72";

        public string PreFieldCarriageReturn { get; set; }
        public string PreFieldLineFeed { get; set; }
        public Field13C Field13C { get; set; }
        public Field20 Field20 { get; set; }
        public Field23B Field23B { get; set; }
        public Field23E Field23E { get; set; }
        public Field32A Field32A { get; set; }
        public Field33B Field33B { get; set; }
        public Field50A Field50A { get; set; }
        public Field50F Field50F { get; set; }
        public Field50K Field50K { get; set; }
        public Field52A Field52A { get; set; }
        public Field52D Field52D { get; set; }
        public Field59 Field59 { get; set; }
        public Field59A Field59A { get; set; }
        public Field59F Field59F { get; set; }
        public Field70 Field70 { get; set; }
        public Field71A Field71A { get; set; }
        public Field71F Field71F { get; set; }
        public Field72 Field72 { get; set; }
        public string EndOfBlockHypen { get; set; }

        public void SetDefaultEndOfBlockHypen()
        {
            EndOfBlockHypen = "-";
        }
        public void SetDefaultPreFieldCrLf()
        {
            PreFieldCarriageReturn = Constants.Cr;
            PreFieldLineFeed = Constants.Lf;
        }
    }
}
