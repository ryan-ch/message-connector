using System;
using System.Collections.Generic;
using System.Text;
using PMC.Common.Logging;
using PMC.Kafka.Producer;

namespace XB.Astrea.Connector.Logger
{
    public class ClientLogger : IClientLogger
    {
        private readonly IKafkaProducer _kafkaProducer;

        public ClientLogger(IKafkaProducer kafkaProducer)
        {
            _kafkaProducer = kafkaProducer;
        }

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
