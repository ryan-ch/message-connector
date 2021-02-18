using System.Linq;
using XB.MtParser.Enums;
using XB.MtParser.Models;

namespace XB.MtParser.Swift_Message
{
    public record UserHeader
    {
        public UserHeader(string userHeaderContent)
        {
            if (string.IsNullOrWhiteSpace(userHeaderContent))
                return;

            var blocks = Block.ParseOneLevelBlocks(userHeaderContent);

            ServiceIdentifier = blocks.FirstOrDefault(block => block.IdentifierAsInt == (int)UserHeaderBlockIdentifiers.ServiceIdentifier)?.Content;
            MessageUserReference = blocks.FirstOrDefault(block => block.IdentifierAsInt == (int)UserHeaderBlockIdentifiers.MessageUserReference)?.Content;
            ServiceTypeIdentifier = blocks.FirstOrDefault(block => block.IdentifierAsInt == (int)UserHeaderBlockIdentifiers.ServiceTypeIdentifier)?.Content;
            ValidationFlag = blocks.FirstOrDefault(block => block.IdentifierAsInt == (int)UserHeaderBlockIdentifiers.ValidationFlag)?.Content;
            UniqueEndToEndTransactionReference = blocks.FirstOrDefault(block => block.IdentifierAsInt == (int)UserHeaderBlockIdentifiers.UniqueEndToEndTransactionReference)?.Content;
        }

        /// <summary>
        /// Field 103: service-identifier. For any message that the user submits to a FINCopy service, block 3 requires an additional field 103.
        /// This field contains a 3-character service identifier that is unique to a specific FINCopy service.
        /// The use of a unique identifier makes it possible to support access to multiple services within the same interface.
        /// The format of field tag 103 is 3!a. Remark for TGT2: If this field is not present,
        /// the message will be delivered directly to the receiver without processing in the Payments Module (PM). 
        /// Present in MT 103, 202, 204. All other MT will not contain field 103.
        /// </summary>
        public string ServiceIdentifier { get; init; }

        /// <summary>
        /// Field 108: message-user-reference. The sender of the message assigns the message user reference (MUR).
        /// If the sender has not defined the message user reference in field 108,
        /// then the system uses the transaction reference number for retrievals and associated system messages and acknowledgements.
        /// The transaction reference number is in field 20 or 20C::SEME of the text block of user-to-user FIN messages.
        /// Field 108 containing only blanks or spaces will be accepted by the system.
        /// </summary>
        public string MessageUserReference { get; init; }

        /// <summary>
        /// Field 111: service-type-identifier. This field identifies the applicable global payment service type.
        /// Field 121 can be present without field 111. Field 111 can only be present if field 121 is also present.
        /// </summary>
        public string ServiceTypeIdentifier { get; init; }

        /// <summary>
        /// Field 119: validation-flag in MX messages. It Indicates whether FIN must perform a special validation.
        /// The following are examples of the values that this field may take: REMIT identifies the presence of field 77T.To use only in MT 103.
        /// RFDD indicates that the message is a request for direct debit.To use only in MT 104. See Error Code C94.
        /// STP indicates that FIN validates the message according to straight-through processing principles.To use only in MTs 102 and 103.
        /// For more information, see Message Format Validation Rules in the SWIFT MT/MX standards
        /// </summary>
        public string ValidationFlag { get; init; }

        /// <summary>
        /// Field 121: unique-end-to-end-transaction-reference. This field provides an end-to-end reference across a payment transaction.
        /// The format of field 121 is xxxxxxxx-xxxx-4xxx-yxxxxxxxxxxxxxxx where x is any hexadecimal character (lower case only) and y is one of 8, 9, a, or b.
        /// Field 121 is mandatory on all MTs 103, 103 STP, 103 REMIT, 202, 202 COV, 205, and 205 COV.See the FIN Service Description for additional information.
        /// Field 121 can be present without field 111. Field 111 can only be present if field 121 is also present.
        /// </summary>
        public string UniqueEndToEndTransactionReference { get; init; }
    }
}