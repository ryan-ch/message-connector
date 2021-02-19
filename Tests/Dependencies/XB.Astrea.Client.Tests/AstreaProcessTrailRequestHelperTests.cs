using Newtonsoft.Json;
using System.Linq;
using XB.Astrea.Client.Messages.Assessment;
using XB.Astrea.Client.Messages.ProcessTrail;
using Xunit;

namespace XB.Astrea.Client.Tests
{

    public class AstreaProcessTrailRequestHelperTests
    {
        public AssessmentRequest AssessmentRequest { get; set; }
        public AssessmentResponse AssessmentResponse { get; set; }
        public Astrea.Client.Messages.ProcessTrail.RequestedProcessTrail RequestedProcessTrail { get; set; }
        public Astrea.Client.Messages.ProcessTrail.OfferedProcessTrail OfferedProcessTrail { get; set; }



        public AstreaProcessTrailRequestHelperTests()
        {
           // SetupAssessmentRequest();
            SetupAssessmentResponse();

            RequestedProcessTrail = new RequestedProcessTrail(AssessmentRequest, AstreaClientTestConstants.Version);
            OfferedProcessTrail = new OfferedProcessTrail(AssessmentResponse, AstreaClientTestConstants.Version, null);
        }

        [Fact(Skip = "not ready")]
        public void ProcessTrails_ParseAssessmentPaymentInstruction_ShouldMapToRequestedProcessTrailPayloads()
        {
            Assert.Equal(AssessmentRequest.PaymentInstructions.First().InstructedDate.ToString("YYYY-MM-dd"),
                RequestedProcessTrail.Payloads.First().Payload.Payment.InstructedDate);
            Assert.Equal(AssessmentRequest.PaymentInstructions.First().Amount,
                RequestedProcessTrail.Payloads.First().Payload.Payment.InstructedAmount);
            Assert.Equal(AssessmentRequest.PaymentInstructions.First().Currency,
                RequestedProcessTrail.Payloads.First().Payload.Payment.InstructedCurrency);
            Assert.Equal(AssessmentRequest.PaymentInstructions.First().DebitAccount.First().Identity,
                    RequestedProcessTrail.Payloads.First().Payload.Payment.DebitAccount.First().Id);
            Assert.Equal(AssessmentRequest.PaymentInstructions.First().CreditAccount.First().Identity,
                RequestedProcessTrail.Payloads.First().Payload.Payment.CreditAccount.First().Id);
        }

        [Fact(Skip = "not ready")]
        public void ProcessTrails_SerializeRequestedProcessTrail_ShouldReturnJson()
        {
            var json = JsonConvert.SerializeObject(RequestedProcessTrail);

            Assert.True(json != string.Empty);
        }

        [Fact(Skip = "not ready")]
        public void ProcessTrails_SerializeOfferedProcessTrail_ShouldReturnJson()
        {
            var json = JsonConvert.SerializeObject(OfferedProcessTrail);

            Assert.True(json != string.Empty);
        }

        //private void SetupAssessmentRequest()
        //{
        //    AssessmentRequest.BasketIdentity = Guid.NewGuid().ToString(); //Mt103->{3:->:121: ???
        //    AssessmentRequest.PaymentInstructions.Add(new PaymentInstruction()
        //    {
        //        InstructedDate = DateTime.Now, //Possible Mt103->{4:->32A->Date..
        //        Amount = 3.14m, //Mt103->{4:->:32A:->Date..Currency..Amount
        //        Currency = "SEK",
        //        Identity = "cd7z1Lja3", //Mt103->{4:->:20: Senders reference
        //        DebitAccount = new List<Messages.Assessment.Account>()
        //        {
        //            //Mt103->{4:->:50K:->rad2
        //            //Mt103->{4:->:50K:->rad1
        //            new Messages.Assessment.Account("seb.payment.se.swift","SE2880000832790000012345")
        //        },
        //        CreditAccount = new List<Messages.Assessment.Account>()
        //        {
        //            //Mt103->{4:->:59:->rad2
        //             //Mt103->{4:->:59:rad1
        //            new Messages.Assessment.Account("seb.payment.se.swift","SE3550000000054910000003")
        //        }
        //    });
        //}

        private void SetupAssessmentResponse()
        {
            AssessmentResponse.Results.Add(new AssessmentResult()
            {

            });
        }
    }
}
