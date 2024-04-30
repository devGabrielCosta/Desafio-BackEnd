using Dominio.Entities;

namespace Dominio.Interfaces.Repositories
{
    public interface IMotoRepository : IBaseRepository<Moto>
    {
        IQueryable<Moto> GetByPlaca(string placa);
        IQueryable<Moto> GetLocacoes();
    }
}
