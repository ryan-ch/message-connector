using System;
using System.Collections.Generic;
using System.Text;

namespace XB.MT.Parser.Model.Text.MT103.Fields
{
    public class AccountCrLF
    {
        public string Account { get; set; }
        public string AccountIndicator { get; set; }
        public string CarriageReturn { get; set; }
        public string LineFeed { get; set; }

        public AccountCrLF(string account, bool setCrLf)
        {
            if (account.StartsWith("/"))
            {
                AccountIndicator = "/";
                if (account.Length > 1)
                {
                    Account = account.Substring(1);
                }
            }
            else
            {
                Account = account;
            }
            if (setCrLf)
            {
                CarriageReturn = "\r";
                LineFeed = "\n";
            }
        }

        public override bool Equals(object o)
        {
            if (o == null || ! this.GetType().Equals(o.GetType()))
            {
                return false;
            }
            else
            {
                AccountCrLF other = (AccountCrLF)o;
                return (Account          == other.Account &&
                        AccountIndicator == other.AccountIndicator &&
                        CarriageReturn   == other.CarriageReturn &&
                        LineFeed         == other.LineFeed);
            }
        }

        public override int GetHashCode()
        {
            return Account.GetHashCode() + AccountIndicator.GetHashCode() + 
                   CarriageReturn.GetHashCode() + LineFeed.GetHashCode();
        }
    }
}
