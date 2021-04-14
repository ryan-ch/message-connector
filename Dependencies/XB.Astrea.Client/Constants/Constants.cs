namespace XB.Astrea.Client.Constants
{
    internal static class AstreaClientConstants
    {
        internal const string System = "HubertAstreaConnector";
        internal const string ProcessTrailSchemaVersion = "v3_3";
        internal const string BoIdType = "sys";
        internal const string ProcessTrailRefType = "bo";
        internal const string ProcessTrailRefIdType = "fcp-access.requestId";
        internal const string PayloadSchemaVersion = "1.0.0";
        internal const string PayloadEncoding = "plain/json";
        internal const string PayloadStore = "ses-fcp-payment-orders";
        internal const string Iban = "iban";
        internal const string Bban = "bban";
        internal const string EventType_Requested = "requested";
        internal const string EventType_Offered = "offered";
        internal const string EventType_Rejected = "rejected";
        internal const string ActorIdType = "swift.bic";
        internal const string ActorRole = "agent";
        internal const string Action_PassThrough = "passthrough";
        internal const string Action_Block = "block";
        internal const string SwedishUtcDateFormat = "yyyy-MM-ddTHH:mm:ss.fff";
        internal const string Hubert_Timeout = "TIMEOUT";
        internal const string Hubert_Accepted = "ACCEPTED";
        internal const string Hubert_Rejected = "REJECTED";
        internal const string Tag121Id = "swift.tag121.uniqueId";
        internal const string Tag20SenderRef = "swift.tag20.sendersRef";
        internal const string Tag72SenderToReceiver = "swift.tag72.senderToReceiver";
        internal const string Tag70RemittanceInfo = "swift.tag70.remittanceInfo";
        internal const string Tag121 = "swift.tag121";
        internal const string IncomingDomestic = "seb.payments.se.incoming.domestic";
        internal const string IncomingXb = "seb.payments.se.incoming.xb";
    }
}
