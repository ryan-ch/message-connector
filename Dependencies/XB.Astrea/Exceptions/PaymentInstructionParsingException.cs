using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XB.Astrea.Client.Exceptions
{
    public class PaymentInstructionParsingException : Exception
    {
        public PaymentInstructionParsingException(string message)
            : base(message)
        {

        }

        public PaymentInstructionParsingException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
