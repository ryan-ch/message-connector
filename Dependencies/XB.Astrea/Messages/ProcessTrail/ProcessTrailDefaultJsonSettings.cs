using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using XB.Astrea.Client.Constants;

namespace XB.Astrea.Client.Messages.ProcessTrail
{
    internal static class ProcessTrailDefaultJsonSettings
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            DateFormatString = AstreaClientConstants.DateFormat
        };
    }
}
