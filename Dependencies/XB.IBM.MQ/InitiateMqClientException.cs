using System;

namespace XB.IBM.MQ
{
    public class InitiateMqClientException : Exception
    {
        public InitiateMqClientException(string message) : base(message)
        {
        }
    }
}
