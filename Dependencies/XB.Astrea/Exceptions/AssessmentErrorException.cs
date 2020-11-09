using System;

namespace XB.Astrea.Client.Exceptions
{
    public class AssessmentErrorException : Exception
    {
        public AssessmentErrorException(string message)
            : base(message)
        {
            
        }

        public AssessmentErrorException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
