namespace Motoflex.Domain.Interfaces.Messaging
{
    public interface IPublisher<T>
    {
        Task PublishAsync(T message);
    }
}
