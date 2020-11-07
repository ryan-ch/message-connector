using System;
using System.Collections.Generic;
using System.Linq;
using XB.Astrea.Client.Assessment;
using Xunit;

namespace XB.Astrea.Client.Tests
{
    
    public class AstreaProcessTrailRequestHelperTests
    {
        public Request Assessment { get; set; } = new Request();

        public AstreaProcessTrailRequestHelperTests()
        {
            Assessment.PaymentInstructions.Add(new PaymentInstruction()
            {
                InstructedDate = DateTime.Now,
                Amount = 100,
                Currency = "SEK",
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

        [Fact]
        public void Parse_ParseAstreaRequestToAstreaProcessTrailRequest_ShouldReturnRequest()
        {
            var processTrail = Astrea.Client.ProcessTrail.RequestHelper.ParseAstreaRequestToAstreaProcessTrailRequest(Assessment, "requested");

            Assert.True(processTrail.Payloads.First().Payment.InstructedDate.Equals(Assessment.PaymentInstructions.First().InstructedDate));
            Assert.True(processTrail.Payloads.First().Payment.InstructedAmount.Equals(Assessment.PaymentInstructions.First().Amount));
            Assert.True(processTrail.Payloads.First().Payment.InstructedCurrency.Equals(Assessment.PaymentInstructions.First().Currency));
        }

    }
}
