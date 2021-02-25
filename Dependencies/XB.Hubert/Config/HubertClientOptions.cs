namespace XB.Hubert.Config
{
    public class HubertClientOptions
    {
        public const string ConfigurationSection = "HubertClient";
        public const string HttpClientIdentifier = "sebcs";

        public string Url { get; set; }
        public string ClientId { get; set; }
    }
}
