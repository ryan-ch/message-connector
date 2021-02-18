using System.Linq;
using XB.MtParser.Enums;

namespace XB.MtParser.Swift_Message
{
    public record ApplicationHeader : SwiftHeaderBase
    {
        private const char InputIdentifier = 'I';
        public const SwiftMessageBlockIdentifiers HeaderType = SwiftMessageBlockIdentifiers.ApplicationHeader;

        public ApplicationHeader(string basicHeaderContent) : base(basicHeaderContent, HeaderType)
        {
            SwiftMessageType = EnumUtil.ParseEnum(basicHeaderContent.Substring(1, 3), SwiftMessageTypes.Unknown);

            InputOutputIdentifier = basicHeaderContent.First();
            if (InputOutputIdentifier == InputIdentifier)
                ParseInputApplicationHeaderContent();
            else
                ParseOutputApplicationHeaderContent();
        }

        /// <summary>
        /// For an input message, the Input/Output Identifier consists of the single letter 'I'
        /// For an output message, the Input/Output Identifier consists of the single letter 'O'
        /// </summary>
        public char InputOutputIdentifier { get; init; }
        /// <summary>
        /// The Message Type consists of 3 digits which define the MT number of the message being input
        /// </summary>
        public SwiftMessageTypes SwiftMessageType { get; init; }
        /// <summary>
        /// Input - This address is the 12-character SWIFT address of the receiver of the message. It defines the destination to which the message should be sent.
        /// </summary>
        public string DestinationAddress { get; private set; }
        /// <summary>
        /// This character, used within FIN Application Headers only, defines the priority with which a message is delivered. The possible values are:
        /// S = System
        /// U = Urgent
        /// N = Normal
        /// </summary>
        public string Priority { get; private set; }
        /// <summary>
        /// Delivery monitoring options apply only to FIN user-to-user messages. The chosen option is expressed as a single digit:
        /// 1 = Non-Delivery Warning
        /// 2 = Delivery Notification
        /// 3 = Non-Delivery Warning and Delivery Notification
        /// If the message has priority 'U', the user must request delivery monitoring option '1' or '3'. If the message has priority 'N', the user can request delivery monitoring option '2' or, by leaving the option blank, no delivery monitoring.
        /// </summary>
        public string DeliveryMonitoring { get; private set; }
        /// <summary>
        /// The obsolescence period defines the period of time after which a Delayed Message (DLM) trailer is added to a FIN user-to-user message when the message is delivered.
        /// For urgent priority messages, it is also the period of time after which, if the message remains undelivered, a Non-Delivery Warning is generated. 
        /// The values for the obsolescence period are: 003 (15 minutes) for 'U' priority, and 020 (100 minutes) for 'N' priority.
        /// </summary>
        public string ObsolescencePeriod { get; private set; }
        /// <summary>
        /// The Input Time local to the sender of the message   
        /// </summary>
        public string InputTime { get; private set; }
        /// <summary>
        /// The MIR consists of four elements:
        /// 1. Sender's Date - Date when the Sender sent the message
        /// 2. The Logical Terminal(LT) Address is a 12-character FIN address.It is the address of the sending LT for this message and includes the Branch Code.It consists of:
        /// - the Sender BIC 8 CODE (8 characters)
        /// - the Logical Terminal Code(1 upper case alphabetic character)
        /// - the Sender BIC Branch Code(3 characters).
        /// It defines the sender of the message to the SWIFT network.
        /// 3. Session number - As appropriate, the current application session number based on the Login.See block 1, field 4
        /// 4. Sequence number - See block 1, field 5
        /// </summary>
        public string MessageInputReference { get; private set; }
        /// <summary>
        /// The output date, local to the receiver
        /// </summary>
        public string OutputDate { get; private set; }
        /// <summary>
        /// The output time, local to the receiver
        /// </summary>
        public string OutputTime { get; private set; }

        public string LTAddress => string.IsNullOrEmpty(MessageInputReference) ? string.Empty : MessageInputReference.Substring(6, 12);

        public string ISOCountryCode => string.IsNullOrEmpty(MessageInputReference) ? string.Empty : MessageInputReference.Substring(10, 2);

        private void ParseInputApplicationHeaderContent()
        {
            DestinationAddress = HeaderContent.Substring(4, 12);
            Priority = HeaderContent.Substring(16, 1);
            DeliveryMonitoring = HeaderContent.Substring(17, 1);
            ObsolescencePeriod = HeaderContent.Substring(18, 3);
        }

        private void ParseOutputApplicationHeaderContent()
        {
            InputTime = HeaderContent.Substring(4, 4);
            MessageInputReference = HeaderContent.Substring(8, 28);
            OutputDate = HeaderContent.Substring(36, 6);
            OutputTime = HeaderContent.Substring(42, 4);
            Priority = HeaderContent.Substring(46, 1);
        }
    }
}