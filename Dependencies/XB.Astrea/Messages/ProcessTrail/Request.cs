using System;
using System.Collections.Generic;
using XB.Astrea.Client.Messages.Assessment;

namespace XB.Astrea.Client.Messages.ProcessTrail
{

    public class Request
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime Time { get; set; } = DateTime.Now;
        public string System { get; set; }
        public Context Context { get; set; }
        public General General { get; set; }
        public List<Payloads> Payloads { get; set; }
    }

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

    public class Event
    {
        public string Type { get; set; } = "requested";
        public Guid Id { get; set; } = Guid.NewGuid();
    }

    public class Location
    {
        public string Type { get; set; }
        public string Id { get; set; }
    }

    public class Ref
    {
        public string Type { get; set; } = "bo";
        public string Id { get; set; }
        public string IdType { get; set; } = "ses.fcp.payment.order.swift";
    }

    public class Context
    {
        public string Cli { get; set; }
        public string Env { get; set; }
    }

    public class Payloads
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

    public class Account
    {
        public string Id { get; set; }
        public string IdType { get; set; } = "iban";
    }

    public class References
    {
        public string Type { get; set; }
        public string Reference { get; set; }
    }
}
