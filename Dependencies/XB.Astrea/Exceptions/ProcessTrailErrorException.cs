using System;

namespace XB.Astrea.Client.Exceptions
{
    public class ProcessTrailErrorException : Exception
    {
        public ProcessTrailErrorException(string message, Exception innerException)
            : base(message, innerException)
        {
            
        }
    }
}
