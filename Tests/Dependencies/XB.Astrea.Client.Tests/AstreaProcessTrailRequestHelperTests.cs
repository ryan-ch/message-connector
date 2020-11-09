using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using XB.Astrea.Client.Messages.Assessment;
using Xunit;

namespace XB.Astrea.Client.Tests
{

    public class AstreaProcessTrailRequestHelperTests
    {
        public Request AssessmentRequest { get; set; } = new Request();
        public Response AssessmentResponse { get; set; } = new Response();
        public Astrea.Client.Messages.ProcessTrail.Requested RequestedProcessTrail { get; set; }
        public Astrea.Client.Messages.ProcessTrail.Offered OfferedProcessTrail { get; set; }



        public AstreaProcessTrailRequestHelperTests()
        {
            SetupAssessmentRequest();
            SetupAssessmentResponse();

            RequestedProcessTrail = Messages.ProcessTrail.ProcessTrailFactory.GetRequestedProcessTrail(AssessmentRequest);
            OfferedProcessTrail = Messages.ProcessTrail.ProcessTrailFactory.GetOfferedProcessTrail(AssessmentResponse);
        }

        [Fact]
        public void ProcessTrails_ParseAssessmentPaymentInstruction_ShouldMapToRequestedProcessTrailPayloads()
        {
            Assert.Equal(AssessmentRequest.PaymentInstructions.First().InstructedDate, 
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

        [Fact]
        public void ProcessTrails_SerializeRequestedProcessTrail_ShouldReturnJson()
        {
            var json = JsonConvert.SerializeObject(RequestedProcessTrail);

            Assert.True(json != string.Empty);
        }

        [Fact]
        public void ProcessTrails_SerializeOfferedProcessTrail_ShouldReturnJson()
        {
            var json = JsonConvert.SerializeObject(OfferedProcessTrail);

            Assert.True(json != string.Empty);
        }

        private void SetupAssessmentRequest()
        {
            AssessmentRequest.BasketIdentity = Guid.NewGuid(); //Mt103->{3:->:121: ???
            AssessmentRequest.PaymentInstructions.Add(new PaymentInstruction()
            {
                InstructedDate = DateTime.Now, //Possible Mt103->{4:->32A->Date..
                Amount = 3.14, //Mt103->{4:->:32A:->Date..Currency..Amount
                Currency = "SEK",
                Identity = "cd7z1Lja3", //Mt103->{4:->:20: Senders reference
                DebitAccount = new List<Account>()
                {
                    new Account()
                    {
                        BankIdentity = "Vårgårda Kromverk", //Mt103->{4:->:50K:->rad2
                        Identity = "SE2880000832790000012345", //Mt103->{4:->:50K:->rad1
                        Type = "seb.payment.se.swift"
                    }
                },
                CreditAccount = new List<Account>()
                {
                    new Account()
                    {
                        BankIdentity = "Volvo Personvagnar Ab", //Mt103->{4:->:59:->rad2
                        Identity = "SE3550000000054910000003", //Mt103->{4:->:59:rad1
                        Type = "seb.payment.se.swift"
                    }
                }
            });
        }

        private void SetupAssessmentResponse()
        {
            AssessmentResponse.Results.Add(new Result()
            {
                
            });
        }
    }
}
