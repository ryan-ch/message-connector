using System;
using System.Collections.Generic;
using System.Text;
using XB.MT.Parser.Model.Common;

namespace XB.MT.Parser.Model.MessageHeader
{
    public class BasicHeader : BlockHeader
    {
        public string AppID { get; set; }
        public string ServiceID { get; set; }
        public string LTAddress { get; set; }
        public string SessionNumber { get; set; }
        public string SequenceNumber { get; set; }
    }
}
