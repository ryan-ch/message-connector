using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XB.Hubert.Config
{
    public class HubertClientOptions
    {
        public const string ConfigurationSection = "HubertClient";

        public string Url { get; set; }
        public string ClientId { get; set; }
    }
}
