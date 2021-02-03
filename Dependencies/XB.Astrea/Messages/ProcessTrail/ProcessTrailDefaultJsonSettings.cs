using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace XB.Astrea.Client.Messages.ProcessTrail
{
    internal class ProcessTrailDefaultJsonSettings
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffZ"
        };
    }
}
