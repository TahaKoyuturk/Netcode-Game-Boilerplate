using Studio.Core.Events;

namespace Studio.Networking
{
    public readonly struct ConnectionLostEvent : IEvent
    {
        public readonly string Reason;

        public ConnectionLostEvent(string reason)
        {
            Reason = reason;
        }
    }

    public readonly struct AllPlayersReadyEvent : IEvent
    {
    }

    public readonly struct NetworkConnectedEvent : IEvent
    {
        public readonly bool IsHost;

        public NetworkConnectedEvent(bool isHost)
        {
            IsHost = isHost;
        }
    }
}
