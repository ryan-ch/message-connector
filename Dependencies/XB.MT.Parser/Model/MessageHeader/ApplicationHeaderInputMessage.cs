namespace XB.MT.Parser.Model.MessageHeader
{
    public class ApplicationHeaderInputMessage : ApplicationHeader
    {
        public const string DestinationAddressKey = "DestinationAddress";
        public const string DeliveryMonitoringKey = "DeliveryMonitoring";
        public const string ObsolescencePeriodKey = "ObsolescencePeriod";

        public string DestinationAddress { get; set; }
        public string DeliveryMonitoring { get; set; }
        public string ObsolescencePeriod { get; set; }
    }
}
