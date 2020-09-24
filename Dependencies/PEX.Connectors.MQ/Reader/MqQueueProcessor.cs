using PEX.Connectors.MQAdapter;
using PMC.Common.Threading;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PEX.Connectors.MQ.Reader
{
    public class MqQueueProcessor : IMqQueueProcessor
    {
        private readonly IMqAdapterFactory _mqAdapterFactory;
        private readonly IMqQueuePoller _mqQueuePoller;
        private readonly ITaskFactory _taskFactory;

        private CancellationTokenSource _cancellationTokenSource;

        public MqQueueProcessor(IMqAdapterFactory mqAdapterFactory, IMqQueuePoller mqQueuePoller, ITaskFactory taskFactory)
        {
            _mqAdapterFactory = mqAdapterFactory;
            _mqQueuePoller = mqQueuePoller;
            _taskFactory = taskFactory;
        }

        public Task Start(Action<MqMessage> proccessMessageFunc, IMqConnectionSettings connectionSettings, string queueName, Dictionary<string, string> properties)
        {
            _cancellationTokenSource = new CancellationTokenSource();

            return _taskFactory.StartNew(() =>
            {
                using (var mqAdapter = _mqAdapterFactory.Create(properties))
                {
                    mqAdapter.Connect(connectionSettings);
                    //do
                    //{
                    _mqQueuePoller.Poll(mqAdapter, proccessMessageFunc, queueName);
                    //    Thread.Sleep(connectionSettings.PollInterval);
                    //} while (!_cancellationTokenSource.IsCancellationRequested);
                }
            }, _cancellationTokenSource, TaskCreationOptions.LongRunning, TaskScheduler.Current, TaskMode.Default);
        }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}