using Dominio.Entities;

namespace Dominio.Interfaces.Repositories
{
    public interface IEntregadorRepository : IBaseRepository<Entregador>
    {
        IQueryable<Entregador> GetLocacoes();
        IQueryable<Entregador> EntregadoresAptosPedido();
    }
}
