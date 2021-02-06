using System;
using System.Collections.Generic;
using System.Text;
using XB.MT.Parser.Model.Common;
using XB.MT.Parser.Parsers.Util.Text.Fields;

namespace XB.MT.Parser.Model.Text.MT103.Fields
{
    public class Field50A : Field50Base
    {
        public AccountCrLf AccountCrLf { get; set; }
        public List<AdditionalRow> IdentifierCodes { get; set; }

        public Field50A(CommonFieldDelimiters commonFieldDelimiters, string fieldValue) : base(commonFieldDelimiters, fieldValue)
        {
            AccountCrLfAdditionalRows accountAndNameAndAdressList = ExtractAccountAndAdditionalRows(fieldValue);
            AccountCrLf = accountAndNameAndAdressList.AccountCrLf;
            IdentifierCodes = accountAndNameAndAdressList.AdditionalRows;
        }

    }
}
