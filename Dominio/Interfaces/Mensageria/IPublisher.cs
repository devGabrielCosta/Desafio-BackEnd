namespace Dominio.Interfaces.Mensageria
{
    public interface IPublisher<T>
    {
        void Publish(T message);
    }
}
