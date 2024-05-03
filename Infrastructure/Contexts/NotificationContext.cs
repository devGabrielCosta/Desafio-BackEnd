using Domain.Interfaces.Notification;

namespace Infrastructure.Contexts
{
    public class NotificationContext : INotificationContext
    {
        private List<string> _notifications;
        public List<string> Notifications { get { return _notifications; } }
        public bool HasNotifications => _notifications.Any();

        public NotificationContext()
        {
            _notifications = new List<string>();
        }

        public void AddNotification(string message)
        {
            _notifications.Add(message);
        }
        public void AddNotification(List<string> messages)
        {
            _notifications.AddRange(messages);
        }
    }
}
