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

            RequestedProcessTrail = Messages.ProcessTrail.RequestHelper.GetRequestedProcessTrail(AssessmentRequest);
            OfferedProcessTrail = Messages.ProcessTrail.RequestHelper.GetOfferedProcessTrail(AssessmentResponse);
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
            AssessmentRequest.BasketIdentity = Guid.NewGuid();
            AssessmentRequest.PaymentInstructions.Add(new PaymentInstruction()
            {
                InstructedDate = DateTime.Now,
                Amount = 100,
                Currency = "SEK",
                Identity = "cd7z1Lja3", //Mt103->{4:->:20:
                DebitAccount = new List<Account>()
                {
                    new Account()
                    {
                        BankIdentity = "BANKIDENTITY-DEBIT",
                        Identity = "IDENTITY-DEBIT",
                        Type = "seb.payment.se.swift"
                    }
                },
                CreditAccount = new List<Account>()
                {
                    new Account()
                    {
                        BankIdentity = "BANKIDENTITY-CREDIT",
                        Identity = "IDENTITY-CREDIT",
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
