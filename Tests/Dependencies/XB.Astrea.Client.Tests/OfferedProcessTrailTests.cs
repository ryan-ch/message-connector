using Moq;
using System;
using System.Collections.Generic;
using Testing.Common.Test_Data;
using XB.Astrea.Client.Constants;
using XB.Astrea.Client.Messages.Assessment;
using XB.Astrea.Client.Messages.ProcessTrail;
using XB.MtParser.Mt103;
using Xunit;

namespace XB.Astrea.Client.Tests
{
    public class OfferedProcessTrailTests
    {
        private readonly Mock<Mt103Message> _mockedMt103;

        public OfferedProcessTrailTests()
        {
            _mockedMt103 = new Mock<Mt103Message>(SwiftMessagesMock.SwiftMessage_2.OriginalMessage, null);
        }

        [Fact]
        public void OfferedProcessTrail_WillSetupFields_WithCorrectData()
        {
            //Arrange
            var assessmentResponse = new AssessmentResponse
            {
                Identity = Guid.NewGuid().ToString(),
                RequestIdentity = Guid.NewGuid().ToString(),
                RiskLevel = "5",
                AssessmentStatus = "succeess",
                Results = new List<AssessmentResult>
                {
                    new AssessmentResult
                    {
                        OrderIdentity = Guid.NewGuid().ToString(),
                        RiskLevel = "5",
                        Extras = new ExtraAssessmentInfo
                        {
                            FullName = "John Doe",
                            AccountNumber = "123456789",
                            OrderingCustomerAccount = "987654321"
                        }
                    }
                }
            };

            //Act
            var offeredProcessTrail = new OfferedProcessTrail(assessmentResponse, string.Empty, _mockedMt103.Object, AstreaClientConstants.Hubert_Accepted);

            //Assert
            Assert.Equal(AstreaClientConstants.EventType_Offered, offeredProcessTrail.General.Event.Type);
            offeredProcessTrail.Payloads.ForEach(payload =>
            {
                Assert.Null(payload.Payload.Reason);
                Assert.Equal(AstreaClientConstants.Action_PassThrough, payload.Payload.Act.RecommendedAction);
                Assert.Equal(AstreaClientConstants.Action_PassThrough, payload.Payload.Act.ExecutedAction);
            });
        }
    }
}
