using System;
using System.Collections.Generic;
using System.Text;

namespace XB.Astrea.Client
{

    public class AstreaProcessTrailRequest
    {
        public DateTime Time { get; set; } = DateTime.Now;
        public General General { get; set; }
        public Guid Id { get; set; } = Guid.NewGuid();
        public Payload[] Payloads { get; set; }
        public Context Context { get; set; }
    }

    public class General
    {
        public Bo Bo { get; set; }
        public DateTime Time { get; set; }
        public Ref[] Refs { get; set; }
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
        public Guid Id { get; set; }
        public string IdType { get; set; } = "ses.fcp.payment.order.swift";
    }

    public class Context
    {
        public string Cli { get; set; }
    }

    public class Payloads
    {
        public string Encoding { get; set; } = "plain/json";
        public string Store { get; set; } = "ses-fcp-payment-orders";
        public Payload Payload { get; set; }
    }

    public class Payload
    {
        public Payment Payment { get; set; }
    }

    public class Payment
    {
        public DateTime InstructedDate { get; set; }
        public Debitaccount[] DebitAccount { get; set; }
        public References[] References { get; set; }
        public Creditaccount[] CreditAccount { get; set; }
        public string InstructedCurrency { get; set; }
        public float InstructedAmount { get; set; }
        public string ExecutionDate { get; set; }
    }

    public class Debitaccount
    {
        public string Id { get; set; }
        public string IdType { get; set; } = "iban";
    }

    public class References
    {
        public string Type { get; set; }
        public string Reference { get; set; }
    }

    public class Creditaccount
    {
        public string Id { get; set; }
        public string IdType { get; set; }
    }
}
