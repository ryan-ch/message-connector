namespace XB.MT.Parser.Model.Text.MT103.Fields
{
    public class PartyIdentifier
    {
        public AccountCrLf AccountCrLf { get; set; }
        public CodeCountryCodeIdentifierCrLf CodeCountryCodeIdentifierCrLf { get; set; }

        public PartyIdentifier(string partyIdentifier)
        {
            if (partyIdentifier[0] == '/')
            {
                AccountCrLf = new AccountCrLf(partyIdentifier, true);
            }
            else
            {
                CodeCountryCodeIdentifierCrLf = new CodeCountryCodeIdentifierCrLf(partyIdentifier, true);
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
                PartyIdentifier other = (PartyIdentifier)o;
                bool acctEquals = false;
                if (AccountCrLf == null)
                {
                    if (other.AccountCrLf == null)
                    {
                        acctEquals = true;
                    }
                    else
                    {
                        acctEquals = false;
                    }
                }
                else
                {
                    acctEquals = AccountCrLf.Equals(other.AccountCrLf);
                }

                bool codeEquals = false;
                if (CodeCountryCodeIdentifierCrLf == null )
                {
                    if (other.CodeCountryCodeIdentifierCrLf == null)
                    {
                        codeEquals = true;
                    }
                    else
                    {
                        codeEquals = false;
                    }
                }
                else
                {
                    codeEquals = CodeCountryCodeIdentifierCrLf.Equals(other.CodeCountryCodeIdentifierCrLf);
                }

                return acctEquals && codeEquals;
            }
        }

    }
}
