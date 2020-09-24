using System;
using PMC.Common.Logging;

namespace XB.Astrea.Connector.Logger
{
    public class ClientLogger : IClientLogger
    {
        public void Log(string logMessage, LogLevel logLevel)
        {
            Console.WriteLine(logMessage);
        }

        public void Log(string logMessage, LogLevel logLevel, DateTime startTime)
        {
            throw new NotImplementedException();
        }

        public void Log(Exception logException, LogLevel logLevel)
        {
            Console.WriteLine(logException.ToString());
        }

        public void Log(string logMessage, Exception logException, LogLevel logLevel)
        {
            throw new NotImplementedException();
        }

        public void Log(string logMessage, Exception logException, LogLevel logLevel, DateTime? startTime, string id)
        {
            throw new NotImplementedException();
        }

        public void Log(string logMessage, LogLevel logLevel, DateTime? startTime, string id)
        {
            throw new NotImplementedException();
        }

        public void Log(string logMessage, LogLevel logLevel, DateTime? endTime, long? duration, string id)
        {
            throw new NotImplementedException();
        }

        public void Log(string logMessage, LogLevel logLevel, DateTime? startTime, DateTime? endTime, long? duration)
        {
            throw new NotImplementedException();
        }

        public void Log(string logMessage, LogLevel logLevel, DateTime? startTime, DateTime? endTime, long? duration, string id)
        {
            throw new NotImplementedException();
        }

        public void Log(Exception logException, LogLevel logLevel, DateTime? startTime, string id)
        {
            throw new NotImplementedException();
        }
    }
}
