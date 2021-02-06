using System;
using System.Collections.Generic;
using System.Text;
using XB.MT.Parser.Model.Text.MT103.Fields;

namespace XB.MT.Parser.Parsers.Util.Text.Fields
{
    public class AccountCrLfAdditionalRows
    {
        public AccountCrLf AccountCrLf { get; set; }
        public List<AdditionalRow> AdditionalRows { get; set; }
    }
}
