﻿namespace XB.MT.Parser.Model.MessageHeader
{
    public class ApplicationHeaderOutputMessage : ApplicationHeader
    {
        public const string InputTimeKey = "InputTime";
        public const string MessageInputReferenceKey = "MessageInputReference";
        public const string OutputDateKey = "OutputDate";
        public const string OutputTimeKey = "OutputTime";

        public string InputTime { get; set; }
        public string MessageInputReference { get; set; }
        public string OutputDate { get; set; }
        public string OutputTime { get; set; }
    }
}
