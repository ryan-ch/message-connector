using System;
using System.Collections.Generic;
using System.Text;

namespace XB.MT.Parser.Model.MessageHeader
{
    public class ApplicationHeaderInputMessage : ApplicationHeader
    {
        public string DestinationAddress { get; set; }
        public string DeliveryMonitoring { get; set; }
        public string ObsolescencePeriod { get; set; }
    }
}
