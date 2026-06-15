namespace Studio.Systems.Notification
{
    public sealed class NotificationData
    {
        public string Title;
        public string Message;
        public float Duration = 3f;
        public NotificationType Type = NotificationType.Info;
    }

    public enum NotificationType
    {
        Info,
        Warning,
        Error,
        Success
    }
}
