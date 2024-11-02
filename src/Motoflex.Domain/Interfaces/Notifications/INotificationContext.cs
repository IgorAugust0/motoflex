namespace Motoflex.Domain.Interfaces.Notifications
{
    public interface INotificationContext
    {
        bool HasNotifications { get; }
        List<string> Notifications { get; }
        void AddNotification(string message);
        void AddNotifications(List<string> messages);
    }
}
