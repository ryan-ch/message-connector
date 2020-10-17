using System;

namespace XB.IBM.MQ.Exceptions
{
    public class InitiateMqClientException : Exception
    {
        public InitiateMqClientException(string message) : base(message)
        {
        }
    }
}
