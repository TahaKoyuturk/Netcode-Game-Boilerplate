using Studio.Core.Events;

namespace Studio.Systems.Loading
{
    public readonly struct LoadingStartedEvent : IEvent
    {
        public readonly string OperationId;

        public LoadingStartedEvent(string operationId)
        {
            OperationId = operationId;
        }
    }

    public readonly struct LoadingProgressEvent : IEvent
    {
        public readonly string OperationId;
        public readonly float Progress;

        public LoadingProgressEvent(string operationId, float progress)
        {
            OperationId = operationId;
            Progress = progress;
        }
    }

    public readonly struct LoadingCompletedEvent : IEvent
    {
        public readonly string OperationId;

        public LoadingCompletedEvent(string operationId)
        {
            OperationId = operationId;
        }
    }
}
