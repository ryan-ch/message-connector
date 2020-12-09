using System;
using System.Collections.Generic;
using XB.Astrea.Client.Messages.Assessment;

namespace XB.Astrea.Client.Messages.ProcessTrail
{
    public abstract class ProcessTrailBase
    {
        private ProcessTrailBase()
        {
            //TODO: Look over how the ids are set and they are referring to correct ids
            Id = Guid.NewGuid();
            Time = DateTime.Now;

            //TODO: System: Get this from configuration or something else! Need to be flexible if another system will use this lib
            System = "Hubert";
            Context = SetupContext();
        }

        protected ProcessTrailBase(AssessmentRequest assessmentRequest) : this()
        {
            General = SetupGeneral(assessmentRequest);
            Payloads = SetupPayloads(assessmentRequest);
        }

        protected ProcessTrailBase(AssessmentResponse assessmentResponse) : this()
        {
            General = SetupGeneral(assessmentResponse);
            Payloads = SetupPayloads(assessmentResponse);
        }

        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime Time { get; set; } = DateTime.Now;
        public string System { get; set; }
        public Context Context { get; set; }
        public General General { get; set; }
        public List<ProcessTrailPayload> Payloads { get; set; }

        protected Context SetupContext()
        {
            //TODO: Cli: is this the application eventId of the system that generates the process trail?
            //TODO: should the env be fetched from the configurations?
            return new Context("Astrea Connector 1.0", "tst");
        }

        protected abstract List<ProcessTrailPayload> SetupPayloads(AssessmentRequest request);
        protected abstract List<ProcessTrailPayload> SetupPayloads(AssessmentResponse response);

        protected abstract General SetupGeneral(AssessmentRequest request);
        protected abstract General SetupGeneral(AssessmentResponse response);
    }

    public record Event(string Type, Guid Id);

    public record Location(string Type, string Id);

    public record Context(string Cli, string Env);

    public record Account(string Id, string IdType);

    public record References(string Type, string Reference);


    public class General
    {
        public Bo Bo { get; set; }
        public DateTime Time { get; set; }
        public List<Ref> Refs { get; set; }
        public Event Event { get; set; }
        public Location Location { get; set; }
    }

    public class Bo
    {
        public string Type { get; set; } = "seb.payments.se.xb.incoming";
        public Guid Id { get; set; } //SUTI or GUID,TBD
        public string IdType { get; set; } = "ses.fcp.payment.order.swift";
    }

    public class Ref
    {
        public string Type { get; set; } = "bo";
        public string Id { get; set; }
        public string IdType { get; set; } = "ses.fcp.payment.order.swift";
    }

    public class ProcessTrailPayload
    {
        public string Id { get; set; }
        public string Encoding { get; set; } = "plain/json";
        public string Store { get; set; } = "ses-fcp-payment-orders";
        public EnvelopPayload Payload { get; set; }
    }

    public class EnvelopPayload
    {
        public Payment Payment { get; set; }
        public Extras Extras { get; set; }
        public Assess Assess { get; set; }
    }

    public class Assess
    {
        public Guid Id { get; set; }
        public int RiskLevel { get; set; }
        public List<Hint> Hints { get; set; }
    }

    public class Extras
    {
        public string SwiftBeneficiaryCustomerAccount { get; set; }
        public string SwiftBeneficiaryCustomerName { get; set; }
        public string SwiftBeneficiaryCustomerAddress { get; set; }
        public string SwiftBeneficiaryBankBIC { get; set; }
        public string SwiftRawMessage { get; set; }
    }

    public class Payment
    {
        public DateTime InstructedDate { get; set; }
        public List<Account> DebitAccount { get; set; }
        public List<References> References { get; set; }
        public List<Account> CreditAccount { get; set; }
        public string InstructedCurrency { get; set; }
        public double InstructedAmount { get; set; }
        public string ExecutionDate { get; set; }
    }
}
