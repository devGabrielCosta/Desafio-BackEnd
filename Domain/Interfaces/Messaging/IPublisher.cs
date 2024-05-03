namespace Domain.Interfaces.Messaging
{
    public interface IPublisher<T>
    {
        void Publish(T message);
    }
}
