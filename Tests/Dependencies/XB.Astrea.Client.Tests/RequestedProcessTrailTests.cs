using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Testing.Common.Test_Data;
using XB.Astrea.Client.Constants;
using XB.Astrea.Client.Messages.Assessment;
using XB.Astrea.Client.Messages.ProcessTrail;
using XB.MtParser.Mt103;
using Xunit;

namespace XB.Astrea.Client.Tests
{
    public class RequestedProcessTrailTests
    {
        [Fact]
        public void RequestedProcessTrail_ShouldInitializeCorrectly()
        {
            // Arrange
            var request = new AssessmentRequest
            {
                OrderIdentity = "dummy order identity",
                BasketIdentity = "dummy basket identity",
                PaymentInstructions = new List<PaymentInstruction>
                {
                    new PaymentInstruction
                    {
                        Identity = "dummy payment instruction identitiy",
                        PaymentType = "dummy payment type",
                        RegistrationTime = DateTime.Now,
                        InstructedDate = DateTime.Now,
                        Amount = 1000,
                        Currency = "SEK",
                        DebitAccount = new Messages.Assessment.Account("SE12345678910"),
                        CreditAccount = new List<Messages.Assessment.Account>
                        {
                            new Messages.Assessment.Account("1234567")
                        },
                        RemittanceInfo = new List<RemittanceInfo>(),
                        InstructionContext = new InstructionContext(new List<string>(), "", "0"),
                    }
                },
                TargetState = AstreaClientConstants.EventType_Requested,
                Tags = new Tags(),
                Mt103Model = new Mock<Mt103Message>(SwiftMessagesMock.SwiftMessage_2.OriginalMessage, null).Object,
                Mt = SwiftMessagesMock.SwiftMessage_2.OriginalMessage
            };

            // Act
            var result = new RequestedProcessTrail(request, string.Empty);

            // Assert
            Assert.Equal(new DateTime(2021, 02, 15, 18, 14, 0), result.General.Time);
            Assert.Equal(request.Mt103Model.UserHeader.UniqueEndToEndTransactionReference, result.General.Bo.Id);
            Assert.Equal("swift.tag121", result.General.Bo.IdType);
            Assert.Equal("seb.payments.se.incoming.xb", result.General.Bo.Type);
            Assert.Equal(new Event(AstreaClientConstants.EventType_Requested,
                $"{request.BasketIdentity}|{new DateTime(2021, 02, 15, 18, 14, 0).ToString(AstreaClientConstants.SwedishUtcDateFormat)}"), result.General.Event);

            Assert.Equal(request.PaymentInstructions.Count, result.Payloads.Count);
            Assert.Equal(result.Id + "-1", result.Payloads[0].Id);

            Assert.Equal(request.PaymentInstructions[0].InstructedDate.ToString("yyyy-MM-dd"), result.Payloads[0].Payload.Payment.InstructedDate);
            Assert.Equal(request.PaymentInstructions[0].Amount, result.Payloads[0].Payload.Payment.InstructedAmount);
            Assert.Equal(request.PaymentInstructions[0].Currency, result.Payloads[0].Payload.Payment.InstructedCurrency);

            Assert.Equal(new References(request.BasketIdentity, "swift.tag121.uniqueId"), result.Payloads[0].Payload.Payment.References.ElementAt(0));
            Assert.Equal(new References(request.Mt103Model.SenderReference, "swift.tag20.sendersRef"), result.Payloads[0].Payload.Payment.References.ElementAt(1));

            Assert.Single(result.Payloads[0].Payload.Payment.RemittanceInfos);
            Assert.Equal(new ProcessTrailRemittanceInfo(request.Mt103Model.RemittanceInformation, "swift.tag70.remittanceInfo"), result.Payloads[0].Payload.Payment.RemittanceInfos.First());

            Assert.Equal(request.PaymentInstructions[0].DebitAccount.Identity, result.Payloads[0].Payload.Payment.DebitAccount.Id);
            Assert.Equal(AstreaClientConstants.Iban, result.Payloads[0].Payload.Payment.DebitAccount.IdType);

            Assert.Equal(request.PaymentInstructions[0].CreditAccount.First().Identity, result.Payloads[0].Payload.Payment.CreditAccount.First().Id);
            Assert.Equal(AstreaClientConstants.Bban, result.Payloads[0].Payload.Payment.CreditAccount.First().IdType);

            Assert.Equal(request.PaymentInstructions.First().CreditAccount.First().Type, result.Payloads.First().Payload.Payment.CreditAccount.First().IdType);
            Assert.Equal(request.PaymentInstructions.First().DebitAccount.Type, result.Payloads.First().Payload.Payment.DebitAccount.IdType);

            Assert.Equal(new Original(request.Mt), result.Payloads[0].Payload.Original);
        }
    }
}
