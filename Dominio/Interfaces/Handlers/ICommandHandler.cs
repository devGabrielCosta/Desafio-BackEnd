namespace Dominio.Interfaces.Handlers
{
    public interface ICommandHandler<Tin, Tout>
    {
        Tout Handle(Tin @event);
    }
    public interface ICommandHandler<Tin>
    {
        void Handle(Tin @event);
    }
}
