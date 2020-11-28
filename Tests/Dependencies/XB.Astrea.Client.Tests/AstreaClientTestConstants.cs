using System;
using System.Collections.Generic;
using System.Text;

namespace XB.Astrea.Client.Tests
{
    public static class AstreaClientTestConstants
    {
        public static string Mt103 =>
            @"{1:F01NKCCXH2NBXXX9712267472}{2:I103CD7BNS1A22WCN}{3:{103:YYG}{108:5V0OP4RFA66}{119:}{111:}{121:7de11583-e6e8-48b2-b8cd-771a839b7fda}}{4:
:20:cd7z1Lja3
:23B:CRED
:32A:200825SEK3,14
:50K:/SE2880000832790000012345
Vårgårda Kromverk
Lilla Korsgatan 3
:59:/SE3550000000054910000003
Volvo Personvagnar Ab
Bernhards Gränd 3, 418 42 Göteborg
:71A:SHA
-}{5:}
$
";

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
