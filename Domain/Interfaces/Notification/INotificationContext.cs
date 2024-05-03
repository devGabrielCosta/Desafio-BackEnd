namespace Domain.Interfaces.Notification
{
    public interface INotificationContext
    {
        bool HasNotifications { get; }
        List<string> Notifications { get; }
        void AddNotification(string message);
        void AddNotification(List<string> messages);
    }
}
