using Studio.Core.Services;
using Studio.Core.Tick;
using Studio.Systems.Notification;

namespace Studio.Managers
{
    public sealed class NotificationManager : IManager, ITickable
    {
        public int TickPriority => 100;

        private readonly NotificationChannel _channel = new();

        public void Initialize()
        {
        }

        public void Shutdown()
        {
        }

        public void Show(NotificationData data)
        {
            _channel.Enqueue(data);
        }

        public void OnTick(float deltaTime)
        {
            _channel.Tick(deltaTime);
        }
    }
}
