using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace XB.MtParser.Mt103
{
    public record OrderingCustomer : ComplexFieldBase
    {
        public OrderingCustomer(string field50A, string field50F, string field50K)
        {
            if (!string.IsNullOrWhiteSpace(field50A))
                ExtractField50A(field50A);
            else if (!string.IsNullOrWhiteSpace(field50F))
                ExtractField50F(field50F);
            else if (!string.IsNullOrWhiteSpace(field50K))
                ExtractField50K(field50K);
        }

        public string Account { get; private set; }
        public string IdentifierCode { get; private set; }
        public string PartyIdentifier { get; private set; }
        public string Name { get; private set; }
        public string Address { get; private set; }
        public string CountryAndTown { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        public string PlaceOfBirth { get; private set; }
        public string CustomerIdentificationNumber { get; private set; }
        public string NationalIdentityNumber { get; private set; }
        public string AdditionalInformation { get; private set; }

        private void ExtractField50A(string f50A)
        {
            // First line with a slash is the account
            Account = ExtractNextLine(f50A, "/", true, out var f50AWithoutAccount);
            // Second line is the identifier code
            IdentifierCode = f50AWithoutAccount;
        }

        private void ExtractField50F(string f50F)
        {
            // First line with a slash is the account
            Account = ExtractNextLine(f50F, "/", false, out _);
            // First line with one of the known codes is PartyIdentifier
            if (string.IsNullOrWhiteSpace(Account))
            {
                var identifierRegex = Regex.Match(f50F, "^(ARNU|CCPT|CUST|DRLC|EMPL|NIDN|SOSE|TXID/.+)$", RegexOptions.Multiline);
                if (identifierRegex.Success)
                    PartyIdentifier = identifierRegex.Groups[1].Value.TrimEnd('\n', '\r');
            }

            // Line with number 1,2 and 3 for Name, Address and CountryAndTown
            (Name, Address, CountryAndTown) = ExtractNameAndAddress(f50F);

            // Line with number 4 is for the date of birth
            var dob = ExtractNextLine(f50F, "4/", false, out _);
            if (DateTime.TryParseExact(dob, "yyyyMMdd", null, DateTimeStyles.None, out var dateOfBirth))
                DateOfBirth = dateOfBirth;

            // Line with number 5 is for the place of birth          
            PlaceOfBirth = ExtractNextLine(f50F, "5/", false, out _);

            // Line with number 6 is for Customer Identification Number
            CustomerIdentificationNumber = ExtractNextLine(f50F, "6/", false, out _);

            // Line with number 7 is for National Identity Number
            NationalIdentityNumber = ExtractNextLine(f50F, "7/", false, out _);

            // Line with number 8 is for Additional Information          
            AdditionalInformation = ExtractNextLine(f50F, "8/", false, out _);
        }

        private void ExtractField50K(string f50K)
        {
            // First line with a slash is the account
            Account = ExtractNextLine(f50K, "/", true, out var f50AWithoutAccount);
            // Second line is the name
            Name = ExtractNextLine(f50AWithoutAccount, "", true, out var f50AWithoutAccountAndName);
            // The rest are the address
            Address = f50AWithoutAccountAndName;
        }
    }
}