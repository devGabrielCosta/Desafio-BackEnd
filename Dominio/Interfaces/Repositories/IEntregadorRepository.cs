using Dominio.Entities;

namespace Dominio.Interfaces.Repositories
{
    public interface IEntregadorRepository : IBaseRepository<Entregador>
    {
        IEnumerable<Entregador> EntregadoresAptosPedido();
    }
}
