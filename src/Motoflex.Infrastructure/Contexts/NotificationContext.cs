using Motoflex.Domain.Interfaces.Notifications;

namespace Motoflex.Infrastructure.Contexts
{
    public class NotificationContext : INotificationContext
    {
        private readonly List<string> _notifications;
        public List<string> Notifications { get { return _notifications; } }
        public bool HasNotifications => _notifications.Count != 0; // Any()

        public NotificationContext()
        {
            _notifications = [];
        }

        public void AddNotification(string message)
        {
            _notifications.Add(message);
        }
        public void AddNotifications(List<string> messages)
        {
            _notifications.AddRange(messages);
        }
    }
}
