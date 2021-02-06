namespace XB.MT.Parser.Model.Text.MT103.Fields
{
    public class PartyIdentifierCrLf
    {
        public PartyIdentifierCrLf(string partyIdentifier)
        {
            if (partyIdentifier[0] == '/')
            {
                AccountCrLf = new AccountCrLf(partyIdentifier, true);
            }
            else
            {
                CodeCountryCodeIdentifier = new CodeCountryCodeIdentifierCrLf(partyIdentifier, true);
            }
        }

        public AccountCrLf AccountCrLf { get; set; }
        public CodeCountryCodeIdentifierCrLf CodeCountryCodeIdentifier { get; set; }
    }
}
