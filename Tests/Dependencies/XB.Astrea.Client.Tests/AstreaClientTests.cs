using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using XB.Astrea.Client.Messages.Assessment;
using Xunit;

namespace XB.Astrea.Client.Tests
{
    public class AstreaClientTests
    {
        public string Mt103 =>
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

        [Fact]
        public void Parse_MtToAstreaRequest_ShouldReturnRequest()
        {
            var request = AssessmentFactory.GetAssessmentRequest(Mt103);

            var requestJson = request.ToJson();

            Assert.True(request.Mt != string.Empty);
            Assert.True(requestJson != string.Empty);
        }
    }
}
