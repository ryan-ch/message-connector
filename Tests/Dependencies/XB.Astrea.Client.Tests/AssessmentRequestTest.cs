using System;
using System.Linq;
using Testing.Common.Test_Data;
using XB.Astrea.Client.Messages.Assessment;
using XB.MtParser.Mt103;
using Xunit;

namespace XB.Astrea.Client.Tests
{
    public class AssessmentRequestTest
    {

        [Fact]
        public void Parse_Mt103ToAstreaRequest_ShouldParseRequestWithCorrectValues()
        {
            var mt103SwiftMessage = new Mt103Message(SwiftMessagesMock.SwiftMessage_2.OriginalMessage, null);
            var request = new AssessmentRequest(mt103SwiftMessage);

            Assert.True(Guid.TryParse(request.OrderIdentity, out var newGuid));
            Assert.Equal("E01EBC0C-0B22-322A-A8F1-097839E991F4", request.BasketIdentity);
            Assert.Equal(SwiftMessagesMock.SwiftMessage_2.OriginalMessage, request.Mt);
            Assert.NotNull(request.Mt103Model);
            Assert.Equal("requested", request.TargetState);
            Assert.Equal(new Tags(), request.Tags);

            var paymentInstruction = request.PaymentInstructions[0];
            Assert.Equal("E01EBC0C-0B22-322A-A8F1-097839E991F4", paymentInstruction.Identity);
            Assert.Equal("seb.payment.se.swift.CRED", paymentInstruction.PaymentType);
            Assert.Equal(DateTime.Now.Date, paymentInstruction.RegistrationTime.Date);
            Assert.Equal(DateTime.Parse("2021-02-15 00:00:00"), paymentInstruction.InstructedDate);
            Assert.Equal(12.00M, paymentInstruction.Amount);
            Assert.Equal("SEK", paymentInstruction.Currency);

            Assert.Equal("DE89370400440532013000", paymentInstruction.DebitAccount.First().Identity);
            Assert.Equal("iban", paymentInstruction.DebitAccount.First().Type);

            Assert.Equal("50601001079", paymentInstruction.CreditAccount.First().Identity);
            Assert.Equal("bban", paymentInstruction.CreditAccount.First().Type);
        }
    }
}
