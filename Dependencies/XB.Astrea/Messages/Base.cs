using System.Collections.Generic;
using Newtonsoft.Json;

namespace XB.Astrea.Client.Messages
{
    public class Hint
    {
        public string Type { get; set; }
        public List<string> Values { get; set; }
    }
}
