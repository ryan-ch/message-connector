using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IBM.WMQ;
using MQMessage = IBM.WMQAX.MQMessage;

namespace PEX.Connectors.MQAdapter
{
    public class MqAdapter : IMqAdapter
    {
        private readonly IMqMessageBuilder _mqMessageBuilder;
        private readonly IMqQueueManagerFactory _mqQueueManagerFactory;
        private IBM.WMQ.MQQueueManager _manager;
        private IBM.WMQ.MQQueue _mqQueuePut;
        private IBM.WMQ.MQQueue _mqQueueGet;
        private Dictionary<string, string> _properties;

        public MqAdapter(IMqMessageBuilder mqMessageBuilder, IMqQueueManagerFactory mqQueueManagerFactory, Dictionary<string, string> properties)
        {
            _mqMessageBuilder = mqMessageBuilder;
            _mqQueueManagerFactory = mqQueueManagerFactory;
            _properties = properties;
        }

        public IMqConnectionSettings MqConnectionSettings { get; private set; }

        public void Connect(IMqConnectionSettings settings)
        {
            if (IsConnected)
            {
                return;
            }

            MqConnectionSettings = settings;

            var properties = new Hashtable
            {
                {MQC.CHANNEL_PROPERTY, settings.Channel},
                {MQC.HOST_NAME_PROPERTY, settings.Hostname},
                {MQC.PORT_PROPERTY, settings.Port},
                {MQC.TRANSPORT_PROPERTY, MQC.TRANSPORT_MQSERIES_MANAGED}
            };

            if (!string.IsNullOrEmpty(settings.SslPath))
            {
                properties.Add(MQC.SSL_CERT_STORE_PROPERTY, settings.SslPath);
                properties.Add(MQC.SSL_CIPHER_SPEC_PROPERTY, settings.SslChyper);
            }

            if(settings.UserName != null) //When debugging IBM MQ in Docker container
            {
                MQEnvironment.UserId = settings.UserName;
                MQEnvironment.Password = settings.Password;
            }

            _manager = _mqQueueManagerFactory.Create(settings.QueueManagerName, properties);
        }

        public bool IsConnected => (_manager != null) && _manager.IsConnected;

        public void Put(MqMessage message, string queueName)
        {
            if (!IsConnected)
            {
                throw new MqAdapterNotConnectedException();
            }

            var mqMessage = _mqMessageBuilder.Build(message);

            const int OPTIONS = MQC.MQOO_OUTPUT | MQC.MQOO_FAIL_IF_QUIESCING;
            const int SET_IDENTITY_OPTIONS = MQC.MQOO_OUTPUT | MQC.MQOO_FAIL_IF_QUIESCING | MQC.MQOO_SET_IDENTITY_CONTEXT;

            var openOptions = MqConnectionSettings.SetIdentityContext ? SET_IDENTITY_OPTIONS : OPTIONS;

            if (_mqQueuePut == null || !_mqQueuePut.IsOpen)
            {
                _mqQueuePut = _manager.AccessQueue(queueName, openOptions);
            }

            var mqPutMsgOpts = new IBM.WMQ.MQPutMessageOptions();

            if (MqConnectionSettings.SetIdentityContext)
            {
                mqPutMsgOpts.Options |= MQC.MQPMO_SET_IDENTITY_CONTEXT;
            }

            mqPutMsgOpts.Options |= MQC.MQPMO_SYNCPOINT;
            _mqQueuePut.Put(mqMessage, mqPutMsgOpts);
            _manager.Commit();
        }

        public MqMessage Get(string applicationId, string queueName)
        {
            if (!IsConnected)
            {
                return null;
            }

            const int OPEN_OPTIONS = MQC.MQOO_INPUT_AS_Q_DEF | MQC.MQOO_FAIL_IF_QUIESCING;

            var mqMessage = new MQMessage { Version = 2, CharacterSet = 1208};

            var mqGetMessageOptions = new IBM.WMQ.MQGetMessageOptions();

            mqGetMessageOptions.Options |= MQC.MQGMO_SYNCPOINT;

            if (!string.IsNullOrEmpty(applicationId))
            {
                mqMessage.ApplicationIdData = applicationId;
                mqGetMessageOptions.Options |= MQC.MQMO_MATCH_MSG_ID;
            }

            try
            {
                if (_mqQueueGet == null || !_mqQueueGet.IsOpen)
                {
                    _mqQueueGet = _manager.AccessQueue(queueName, OPEN_OPTIONS);
                }

                _mqQueueGet.Get(mqMessage, mqGetMessageOptions);
            }
            catch (IBM.WMQ.MQException mqException)
            {
                if (mqException.Reason == MQC.MQRC_NO_MSG_AVAILABLE)
                {
                    return null;
                }
                throw;
            }

            var data = string.Empty;
            if(mqMessage.MessageLength > 0)
            {
                data = mqMessage.ReadString(mqMessage.MessageLength);
            }

            var message = new MqMessage(data, mqMessage.ApplicationIdData.TrimEnd())
            {
                BackoutCount = mqMessage.BackoutCount,
                MessageId = mqMessage.MessageIdString,
                Properties = GetProperties(mqMessage, _properties)
            };

        var correlationGuid = GetCorrelationId(mqMessage);

            if (correlationGuid != Guid.Empty)
            {
                message.CorrelationId = correlationGuid;
            }

            return message;
        }

        public void Commit()
        {
            _manager.Commit();
        }

        public void Backout()
        {
            _manager.Backout();
        }

        private static Guid GetCorrelationId(MQMessage mqMessage)
        {
            return new Guid(mqMessage.CorrelationId.Take(16).ToArray());
        }

        public int GetQueueDepth(string queueName)
        {
            if (!IsConnected)
            {
                return 0;
            }

            const int OPEN_OPTIONS = MQC.MQOO_INPUT_AS_Q_DEF | MQC.MQOO_FAIL_IF_QUIESCING | MQC.MQOO_INQUIRE;
            _mqQueueGet = _manager.AccessQueue(queueName, OPEN_OPTIONS);

            return _mqQueueGet.CurrentDepth;
        }

        public void Dispose()
        {
            //Do nothing. Hold _manager to MQ connected
        }

        private Dictionary<string, string> GetProperties(MQMessage mqMessage, Dictionary<string, string> properties)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            if (properties != null)
            {
                foreach (var property in properties)
                {
                    try
                    {
                        result.Add(property.Key, mqMessage.GetStringProperty(property.Value));
                    }
                    catch // Some properties are only set if theres an error in the message, if not GetStringProperty throws exception.
                    {
                        result.Add(property.Key, string.Empty);
                    }
                }
            }

            return result;
        }
    }
}