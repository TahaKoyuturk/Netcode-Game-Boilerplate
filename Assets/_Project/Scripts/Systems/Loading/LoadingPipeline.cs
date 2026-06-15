using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Studio.Core.Events;
using Studio.Core.Services;

namespace Studio.Systems.Loading
{
    public sealed class LoadingPipeline : IService
    {
        private readonly Queue<LoadingOperation> _queue = new();
        private bool _isRunning;

        public bool IsRunning => _isRunning;

        public void Enqueue(LoadingOperation operation)
        {
            _queue.Enqueue(operation);
        }

        public async Task RunAsync()
        {
            if (_isRunning)
            {
                return;
            }

            _isRunning = true;

            while (_queue.Count > 0)
            {
                var operation = _queue.Dequeue();
                EventBus.Publish(new LoadingStartedEvent(operation.Id));

                var progress = new Progress<float>(value =>
                    EventBus.Publish(new LoadingProgressEvent(operation.Id, value)));

                await operation.ExecuteAsync(progress);
                EventBus.Publish(new LoadingCompletedEvent(operation.Id));
            }

            _isRunning = false;
        }
    }
}
