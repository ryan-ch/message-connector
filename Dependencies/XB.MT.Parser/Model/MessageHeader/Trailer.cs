using XB.MT.Common.Model.Common;
using XB.MT.Common.Model.Tags;

namespace XB.MT.Parser.Model.MessageHeader
{
    public class Trailer : BlockHeader
    {
        public const string CHKKey = "CHK";
        public const string TNGKey = "TNG";
        public const string PDEKey = "PDE";
        public const string DLMKey = "DLM";
        public const string MRFKey = "MRF";
        public const string PDMKey = "PDM";
        public const string SYSKey = "SYS";


        public TagChecksum Tag_Checksum { get; set; }
        public TagTestAndTrainingMessage Tag_TestAndTrainingMessage { get; set; }
        public TagPossibleDuplicateEmission Tag_PossibleDuplicateEmission { get; set; }
        public TagDelayedMessage Tag_DelayedMessage { get; set; }
        public TagMessageReference Tag_MessageReference { get; set; }
        public TagPossibleDuplicateMessage Tag_PossibleDuplicateMessage { get; set; }
        public TagSystemOriginatedMessage Tag_SystemOriginatedMessage { get; set; }


        public class TagChecksum : TagHeader
        {
            public TagChecksum(CommonBlockDelimiters commonTagDelimiters, string tagValue) : base(commonTagDelimiters)
            {
                Checksum = tagValue;
            }

            public string Checksum { get; set; }
        }

        public class TagTestAndTrainingMessage : TagHeader
        {
            public TagTestAndTrainingMessage(CommonBlockDelimiters commonTagDelimiters) : base(commonTagDelimiters)
            {
            }
        }

        public class TagPossibleDuplicateEmission : TagHeader
        {
            public TagPossibleDuplicateEmission(CommonBlockDelimiters commonTagDelimiters, string tagValue) : base(commonTagDelimiters)
            {
                PossibleDuplicateEmission = tagValue;
            }

            public string PossibleDuplicateEmission { get; set; }
        }

        public class TagDelayedMessage : TagHeader
        {
            public TagDelayedMessage(CommonBlockDelimiters commonTagDelimiters) : base(commonTagDelimiters)
            {
            }
        }

        public class TagMessageReference : TagHeader
        {
            public TagMessageReference(CommonBlockDelimiters commonTagDelimiters, string tagValue) : base(commonTagDelimiters)
            {
                MessageReference = tagValue;
            }

            public string MessageReference { get; set; }
        }

        public class TagPossibleDuplicateMessage : TagHeader
        {
            public TagPossibleDuplicateMessage(CommonBlockDelimiters commonTagDelimiters, string tagValue) : base(commonTagDelimiters)
            {
                PossibleDuplicateMessage = tagValue;
            }

            public string PossibleDuplicateMessage { get; set; }
        }

        public class TagSystemOriginatedMessage : TagHeader
        {
            public TagSystemOriginatedMessage(CommonBlockDelimiters commonTagDelimiters, string tagValue) : base(commonTagDelimiters)
            {
                SystemOriginatedMessage = tagValue;
            }

            public string SystemOriginatedMessage { get; set; }
        }
    }
}
