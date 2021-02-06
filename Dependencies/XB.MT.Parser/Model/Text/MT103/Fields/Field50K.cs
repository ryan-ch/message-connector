﻿using System.Collections.Generic;
using XB.MT.Parser.Model.Common;
using XB.MT.Parser.Parsers.Util.Text.Fields;

namespace XB.MT.Parser.Model.Text.MT103.Fields
{
    public class Field50K : Field50Base
    {
        public AccountCrLf AccountCrLf { get; set; }
        public List<AdditionalRow> NameAndAddresses { get; set; }

        public Field50K(CommonFieldDelimiters commonFieldDelimiters, string fieldValue) : base(commonFieldDelimiters, fieldValue)
        {
            AccountCrLfAdditionalRows accountAndNameAndAdressList = ExtractAccountAndAdditionalRows(fieldValue);
            AccountCrLf = accountAndNameAndAdressList.AccountCrLf;
            NameAndAddresses = accountAndNameAndAdressList.AdditionalRows;
        }
    }
}
