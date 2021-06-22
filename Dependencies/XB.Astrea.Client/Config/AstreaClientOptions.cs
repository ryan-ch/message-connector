using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using XB.Astrea.Client.Constants;

namespace XB.Astrea.Client.Config
{
    public record AstreaClientOptions
    {
        public const string ConfigurationSection = "AstreaClient";

        public static readonly JsonSerializerSettings ProcessTrailDefaultJsonSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            DateFormatString = AstreaClientConstants.SwedishUtcDateFormat
        };

        public string Url { get; set; }
        public string FraudEndpoint { get; set; }
        public string Version { get; set; }
        public List<string> AcceptableTransactionTypes { get; set; }
    }
}
