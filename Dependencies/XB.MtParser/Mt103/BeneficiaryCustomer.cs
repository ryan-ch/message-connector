namespace XB.MtParser.Mt103
{
    public record BeneficiaryCustomer : ComplexFieldBase
    {
        public BeneficiaryCustomer(string field59, string field59A, string field59F)
        {
            if (!string.IsNullOrWhiteSpace(field59))
                ExtractField59(field59);
            else if (!string.IsNullOrWhiteSpace(field59A))
                ExtractField59A(field59A);
            else if (!string.IsNullOrWhiteSpace(field59F))
                ExtractField59F(field59F);
        }

        public string Account { get; private set; }
        public string IdentifierCode { get; private set; }
        public string Name { get; private set; }
        public string Address { get; private set; }
        public string CountryAndTown { get; private set; }

        private void ExtractField59(string f59)
        {
            // First line with a slash is the account
            Account = ExtractNextLine(f59, "/", true, out var f59AWithoutAccount);
            // Second line is the name
            Name = ExtractNextLine(f59AWithoutAccount, "", true, out var f59AWithoutAccountAndName);
            // The rest are the address
            Address = f59AWithoutAccountAndName;
        }

        private void ExtractField59A(string f59A)
        {
            // First line with a slash is the account
            Account = ExtractNextLine(f59A, "/", true, out var f50AWithoutAccount);
            // Second line is the identifier code
            IdentifierCode = f50AWithoutAccount;
        }

        private void ExtractField59F(string f59F)
        {
            // First line with a slash is the account
            Account = ExtractNextLine(f59F, "/", false, out _);
            // Line with number 1,2 and 3 for Name, Address and CountryAndTown
            (Name, Address, CountryAndTown) = ExtractNameAndAddress(f59F);
        }
    }
}