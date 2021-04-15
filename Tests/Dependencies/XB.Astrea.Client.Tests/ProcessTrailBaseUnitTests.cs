using Microsoft.Extensions.Logging.Abstractions;
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
    public class ProcessTrailBaseUnitTests
    {
        private const string AppVersion = "1.12";
        private readonly ProcessTrailBaseMock _processTrailBaseMock;
        private readonly AssessmentResponse _assessmentResponse = new AssessmentResponse
        {
            Results = new List<AssessmentResult>
            {
                new AssessmentResult
                {
                    RiskLevel = "6",
                    OrderIdentity = "Id1",
                    Hints = new[] {new Hint("Hint1", new[] {"Value11", "Value12"})},
                    Extras = new ExtraAssessmentInfo {AccountNumber = "Account1", FullName = "Name1", OrderingCustomerAccount = "OrderingCustomerAccount1"}
                },
                new AssessmentResult
                {
                    RiskLevel = "2",
                    OrderIdentity = "Id2",
                    Hints = new[] {new Hint("Hint2", new[] {"Value21", "Value22"})},
                    Extras = new ExtraAssessmentInfo {AccountNumber = "Account2", FullName = "Name2", OrderingCustomerAccount = "OrderingCustomerAccount2"}
                }
            }
        };
        public ProcessTrailBaseUnitTests()
        {
            _processTrailBaseMock = new ProcessTrailBaseMock(AppVersion);
        }

        [Fact]
        public void ProcessTrailBase_WillInitiateValues_Correctly()
        {
            // Arrange
            // Act
            // Assert
            Assert.NotEqual(default, _processTrailBaseMock.Id);
            Assert.NotEqual(default, _processTrailBaseMock.Time);
            Assert.Equal(AstreaClientConstants.System, _processTrailBaseMock.System);
            Assert.Equal($"{AstreaClientConstants.System}-v{AppVersion}", _processTrailBaseMock.Context.Cli);
            Assert.Equal(AstreaClientConstants.ProcessTrailSchemaVersion, _processTrailBaseMock.Context.Sch);
        }

        [Fact]
        public void SetupPayloads_WillInitiateValues_Correctly()
        {
            // Arrange
            const string act = "Fake Act";

            // Act
            var payloads = _processTrailBaseMock.SetupPayloads(_assessmentResponse, Mt103MessagesMock.Mt103Message_02, act, "randomStatus");

            // Assert
            for (var i = 0; i < payloads.Count; i++)
            {
                Assert.EndsWith("-1", payloads[i].Id);
                Assert.Equal(new Act(act, act), payloads[i].Payload.Act);
                Assert.Null(payloads[i].Payload.Reason);
                Assert.Equal(2, payloads[i].Payload.Payment.References.Count());
                Assert.Single(payloads[i].Payload.Payment.RemittanceInfos);
                Assert.Equal(_assessmentResponse.Results[i].OrderIdentity, payloads[i].Payload.Assess.Id);
                Assert.Equal(_assessmentResponse.Results[i].RiskLevel, payloads[i].Payload.Assess.RiskLevel.ToString());
                Assert.Equal(_assessmentResponse.Results[i].Hints, payloads[i].Payload.Assess.Hints);
                Assert.Equal(new AssessExtras
                {
                    BeneficiaryCustomerAccount = _assessmentResponse.Results[i].Extras.AccountNumber,
                    BeneficiaryCustomerName = _assessmentResponse.Results[i].Extras.FullName,
                    OrderingCustomerAccount = _assessmentResponse.Results[i].Extras.OrderingCustomerAccount
                },
                    payloads[i].Payload.Assess.Extras);
                Assert.Equal(new Original(SwiftMessagesMock.SwiftMessage_2.OriginalMessage), payloads[i].Payload.Original);
            }
        }

        [Fact]
        public void SetupPayloads_WillInitiateReasonAndHints_BasedOnHubertStatus()
        {
            // Arrange Hubert Accepted
            const string act = "Fake Act";
            var hubertStatus = AstreaClientConstants.Hubert_Accepted;

            // Act
            var payloads = _processTrailBaseMock.SetupPayloads(_assessmentResponse, Mt103MessagesMock.Mt103Message_02, act, hubertStatus);

            // Assert
            payloads.ForEach(p => Assert.Null(p.Payload.Reason));

            // Arrange Hubert Rejected
            hubertStatus = AstreaClientConstants.Hubert_Rejected;

            // Act
            payloads = _processTrailBaseMock.SetupPayloads(_assessmentResponse, Mt103MessagesMock.Mt103Message_02, act, hubertStatus);

            // Assert
            payloads.ForEach(p => Assert.Equal(new Reason("fcp-access", "HUB_MT103_REJECTED"), p.Payload.Reason));

            // Arrange Hubert Timeout
            hubertStatus = AstreaClientConstants.Hubert_Timeout;

            // Act
            payloads = _processTrailBaseMock.SetupPayloads(_assessmentResponse, Mt103MessagesMock.Mt103Message_02, act, hubertStatus);

            // Assert
            payloads.ForEach(p =>
            {
                Assert.Equal(new Reason("timeout", "Timeout approval decision received in response"), p.Payload.Reason);
                Assert.Contains(p.Payload.Assess.Hints, h => h.Name == "timeout" && h.Values.Contains("HUBERT_TIMEOUT"));
            });
        }

        [Fact]
        public void SetupGeneral_WillInitiateValues_Correctly()
        {
            // Arrange
            const string eventType = "Test Event";

            // Act
            var general = _processTrailBaseMock.SetupGeneral(eventType, _assessmentResponse, Mt103MessagesMock.Mt103Message_01);

            // Assert
            Assert.Equal(Mt103MessagesMock.Mt103Message_01.ApplicationHeader.OutputDate, general.Time);
            Assert.NotEqual(default, general.Bo);
            Assert.Equal(new Ref(_assessmentResponse.Identity, AstreaClientConstants.ProcessTrailRefType, AstreaClientConstants.ProcessTrailRefIdType), general.Refs.First());
            Assert.Equal(eventType, general.Event.Type);
            Assert.Contains(Mt103MessagesMock.Mt103Message_01.UserHeader.UniqueEndToEndTransactionReference, general.Event.Id);
            Assert.Contains(Mt103MessagesMock.Mt103Message_01.ApplicationHeader.OutputDate.ToString("s"), general.Event.Id);
        }

        [Theory]
        [InlineData("", AstreaClientConstants.IncomingXb)]
        [InlineData("SomeInfo", AstreaClientConstants.IncomingXb)]
        [InlineData("/DOM-ExtraInfo", AstreaClientConstants.IncomingDomestic)]
        public void GetBo_WillInitiateValues_Correctly(string senderToReceiverInformation, string expectedType)
        {
            // Arrange
            const string identity = "Fake Id";

            // Act
            var bo = _processTrailBaseMock.GetBo(identity, senderToReceiverInformation);

            // Assert
            Assert.Equal(new Bo(identity, expectedType), bo);
        }

        [Fact]
        public void GetReferences_WillInitiateValues_Correctly()
        {
            // Arrange
            const string identity = "Fake Id";
            const string senderRef = "Test ref";

            // Act
            var references = _processTrailBaseMock.GetReferences(identity, senderRef).ToArray();

            // Assert
            Assert.Equal(new References(identity, AstreaClientConstants.Tag121Id), references[0]);
            Assert.Equal(new References(senderRef, AstreaClientConstants.Tag20SenderRef), references[1]);
        }

        [Fact]
        public void GetRemittanceInfos_IfNoInfoProvided_ItWillReturnEmpty()
        {
            // Arrange No Info
            var mt103 = new Mt103Message(SwiftMessagesMock.SwiftMessage_2.OriginalMessage.Replace(":70:BETORSAK", ""), NullLogger<MtParser.MtParser>.Instance);

            // Act
            var remittanceInfo = _processTrailBaseMock.GetRemittanceInfos(mt103);

            // Assert
            Assert.Empty(remittanceInfo);
        }

        [Fact]
        public void GetRemittanceInfos_IfInfoProvided_ItWillReturnCorrectData()
        {
            // Arrange RemittanceInformation only
            // Act
            var remittanceInfo = _processTrailBaseMock.GetRemittanceInfos(Mt103MessagesMock.Mt103Message_02).ToArray();

            // Assert
            Assert.Equal(new ProcessTrailRemittanceInfo(Mt103MessagesMock.Mt103Message_02.RemittanceInformation, AstreaClientConstants.Tag70RemittanceInfo), remittanceInfo[0]);
            Assert.Single(remittanceInfo);

            // Arrange SenderToReceiverInformation only
            // Act
            remittanceInfo = _processTrailBaseMock.GetRemittanceInfos(Mt103MessagesMock.Mt103Message_01).ToArray();

            // Assert
            Assert.Equal(new ProcessTrailRemittanceInfo(Mt103MessagesMock.Mt103Message_01.SenderToReceiverInformation, AstreaClientConstants.Tag72SenderToReceiver), remittanceInfo[0]);
            Assert.Single(remittanceInfo);

            // Arrange both RemittanceInformation and SenderToReceiverInformation
            var mt103 = new Mt103Message(SwiftMessagesMock.SwiftMessage_2.OriginalMessage.Replace(":70:BETORSAK", ":70:BETORSAK\r\n:72:/REC/RETN"), NullLogger<MtParser.MtParser>.Instance);

            // Act
            remittanceInfo = _processTrailBaseMock.GetRemittanceInfos(mt103).ToArray();

            // Assert
            Assert.Equal(new ProcessTrailRemittanceInfo(mt103.SenderToReceiverInformation, AstreaClientConstants.Tag72SenderToReceiver), remittanceInfo[0]);
            Assert.Equal(new ProcessTrailRemittanceInfo(mt103.RemittanceInformation, AstreaClientConstants.Tag70RemittanceInfo), remittanceInfo[1]);
        }

        private class ProcessTrailBaseMock : ProcessTrailBase
        {
            public ProcessTrailBaseMock(string appVersion) : base(appVersion) { }

            public new List<ProcessTrailPayload> SetupPayloads(AssessmentResponse response, Mt103Message parsedMt, string actAction, string hubertStatus) =>
                base.SetupPayloads(response, parsedMt, actAction, hubertStatus);

            public new General SetupGeneral(string eventType, AssessmentResponse response, Mt103Message parsedMt) =>
                base.SetupGeneral(eventType, response, parsedMt);

            public new Bo GetBo(string identity, string senderToReceiverInformation) =>
                base.GetBo(identity, senderToReceiverInformation);

            public new IEnumerable<References> GetReferences(string identity, string senderReference) =>
                base.GetReferences(identity, senderReference);

            public new IEnumerable<ProcessTrailRemittanceInfo> GetRemittanceInfos(Mt103Message mt103) =>
                base.GetRemittanceInfos(mt103);
        }
    }
}
