using System;
using System.Collections.Generic;
using System.Text;
using XB.MT.Parser.Model.Text.MT103.Fields;
using XB.MT.Parser.Model.Common;
using XB.MT.Parser.Model.MessageHeader;

namespace XB.MT.Parser.Model.Text.MT103
{

    public class MT103SingleCustomerCreditTransferText : BlockHeader
    {
        public string PreFieldCarriageReturn { get; set; }
        public string PreFieldLineFeed { get; set; }
        public Field13C Field13C { get; set; }
        public Field20 Field20 { get; set; }
        public Field23B Field23B { get; set; }
        public Field23E Field23E { get; set; }
        public Field32A Field32A { get; set; }
        public Field33B Field33B { get; set; }
        public Field50K Field50K { get; set; }
        public Field52A Field52A { get; set; }
        public Field59 Field59 { get; set; }
        public Field70 Field70 { get; set; }
        public Field71A Field71A { get; set; }
        public Field71F Field71F { get; set; }
        public string EndOfBlockHypen { get; set; }

        public void SetDefaultEndOfBlockHypen()
        {
            EndOfBlockHypen = "-";
        }
        public void SetDefaultPreFieldCrLf()
        {
            PreFieldCarriageReturn = "\r";
            PreFieldLineFeed = "\n";
        }
    }
}
