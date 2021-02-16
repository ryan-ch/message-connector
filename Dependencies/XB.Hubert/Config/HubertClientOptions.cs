using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace XB.Hubert.Config
{
    public class HubertClientOptions
    {
        public const string ConfigurationSection = "HubertClient";

        public string Url { get; set; }
        public string ClientId { get; set; }
        public string JwtUrl { get; set; }
        public string JwtClientId { get; set; }
        public string JwtClientSecret { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
