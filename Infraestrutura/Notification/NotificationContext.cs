using Dominio.Interfaces.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestrutura.Notification
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
