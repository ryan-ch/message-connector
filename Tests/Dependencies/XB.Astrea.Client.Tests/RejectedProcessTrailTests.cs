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
    public class RejectedProcessTrailTests
    {
        [Fact]
        public void RejectedProcessTrail_ShouldInitializeCorrectly()
        {
            // Arrange
            var assessmentResponse = new AssessmentResponse
            {
                Identity = Guid.NewGuid().ToString(),
                RequestIdentity = Guid.NewGuid().ToString(),
                RiskLevel = "6",
                AssessmentStatus = "fail",
                Results = new List<AssessmentResult>
                {
                    new AssessmentResult
                    {
                        OrderIdentity = Guid.NewGuid().ToString(),
                        RiskLevel = "6",
                        Extras = new ExtraAssessmentInfo
                        {
                            FullName = "John Doe",
                            AccountNumber = "123456789",
                            OrderingCustomerAccount = "987654321"
                        }
                    }
                }
            };
            var mockedMt103 = new Mock<Mt103Message>(SwiftMessagesMock.SwiftMessage_2.OriginalMessage, null);

            // Act
            var rejectedProcessTrail = new RejectedProcessTrail(assessmentResponse, string.Empty, mockedMt103.Object);

            // Assert
            Assert.Equal(AstreaClientConstants.EventType_Rejected, rejectedProcessTrail.General.Event.Type);
            Assert.Equal(assessmentResponse.Results.Count, rejectedProcessTrail.Payloads.Count);

            Assert.Equal("fcp-access", rejectedProcessTrail.Payloads[0].Payload.Reason.Code);
            Assert.Equal("HUB_MT103_REJECTED", rejectedProcessTrail.Payloads[0].Payload.Reason.Text);

            Assert.Equal(AstreaClientConstants.Action_Block, rejectedProcessTrail.Payloads[0].Payload.Act.RecommendedAction);
            Assert.Equal(AstreaClientConstants.Action_Block, rejectedProcessTrail.Payloads[0].Payload.Act.ExecutedAction);

            Assert.Equal(assessmentResponse.Results[0].OrderIdentity, rejectedProcessTrail.Payloads[0].Payload.Payment.References.First().Reference);

            Assert.Equal(assessmentResponse.Results[0].Extras.AccountNumber, rejectedProcessTrail.Payloads[0].Payload.Assess.Extras.BeneficiaryCustomerAccount);
            Assert.Equal(assessmentResponse.Results[0].Extras.FullName, rejectedProcessTrail.Payloads[0].Payload.Assess.Extras.BeneficiaryCustomerName);
            Assert.Equal(assessmentResponse.Results[0].Extras.OrderingCustomerAccount, rejectedProcessTrail.Payloads[0].Payload.Assess.Extras.OrderingCustomerAccount);
        }
    }
}
