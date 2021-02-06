using System;
using System.Collections.Generic;
using XB.MT.Parser.Model.Text.MT103.Fields;
using XB.MT.Parser.Parsers.Util.Text.Fields;

namespace XB.MT.Parser.Model.Common
{
    public class Field
    {
        public CommonFieldDelimiters CommonFieldDelimiters { get; set; }

        public Field(CommonFieldDelimiters commonFieldDelimiters)
        {
            CommonFieldDelimiters = commonFieldDelimiters;
        }

        internal string[] SplitFieldByCrLf(string fieldValue)
        {
            return SplitField(fieldValue, new string[] { Constants.CrLf });
        }
        internal string[] SplitField(string fieldValue, string[] splitValues)
        {
            if (fieldValue != null)
            {
                return fieldValue.Split(splitValues, StringSplitOptions.None);
            }
            else
            {
                return null;
            }
        }

        protected AccountCrLfAdditionalRows ExtractAccountAndAdditionalRows(string fieldValue)
        {
            AccountCrLfAdditionalRows accountAndRows = new AccountCrLfAdditionalRows();
            accountAndRows.AccountCrLf = null;
            accountAndRows.AdditionalRows = null;

            string[] parts = SplitFieldByCrLf(fieldValue);
            if (parts != null)
            {
                bool firstPart = true;
                foreach (var part in parts)
                {
                    if (firstPart && part[0] == '/')
                    {
                        accountAndRows.AccountCrLf = new AccountCrLf(part, true);
                    }
                    else
                    {
                        if (accountAndRows.AdditionalRows == null)
                        {
                            accountAndRows.AdditionalRows = new List<AdditionalRow>();
                        }
                        accountAndRows.AdditionalRows.Add(new AdditionalRow(part, true));
                    }
                    firstPart = false;
                }
            }

            return accountAndRows;
        }
    }
}
