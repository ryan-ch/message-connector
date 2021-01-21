namespace XB.Astrea.Client.Tests
{
    public static class AstreaClientTestConstants
    {
        public const string Version = "1.0.0";
        public static string Mt103 =>
            @"{1:F01ESSESES0AXXX8000019102}{2:O1030955100518CIBCCATTAXXX76763960792012021544N}{3:{108:78}{111:001}{121:98408af9-695e-4828-9b63-24e63cacb8eb}}{4:
:20:XMEC-CAD
:23B:CRED
:32A:200723CAD224,28
:33B:CAD224,28
:50K:/9991
ONE OF OUR CUSTOMERS
:59:/52801010480
BENEF
:70:BETORSAK
:71A:SHA
-}{S:{MAN:UAKOUAK4600}}";

        public static string AstreaResponseAsString =>
            @"{
  ""requestIdentity"": ""a9ad57c5-5807-4d51-848e-dcd2f64ca6ef"",
  ""identity"": ""46bcae31-b14f-4979-a78a-d255d57ac0b4"",
  ""riskLevel"": ""7"",
  ""hints"": [{
    ""type"": ""reason"",
    ""values"": [
      ""Deviating_Payment""
    ]
  }],
  ""assessmentStatus"": ""OK"",
  ""riskyInstructions"": [
    ""cd7z1Lja3""
  ],
  ""results"": [{
    ""orderIdentity"": ""cd7z1Lja3"",
    ""riskLevel"": ""7"",
    ""hints"": [{
      ""type"": ""reason"",
      ""values"": [
        ""Deviating_Payment""
      ]
    }],
    ""extras"": {
      ""accountHolderID"": ""NA"",
      ""soleProprietorship"": ""false"",
      ""orderingCustomerAddress"": ""NA"",
      ""orderingBankBIC"": ""Vårgårda Kromverk"",
      ""fullName"": ""NA"",
      ""orderingCustomerAccount"": ""SE2880000832790000012345"",
      ""orderingCustomerName"": ""NA"",
      ""accountNumber"": ""NA"",
      ""physical"": ""false""
    }
  }]
}
";
    }
}
