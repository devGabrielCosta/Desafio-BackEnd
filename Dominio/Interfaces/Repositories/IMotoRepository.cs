using Dominio.Entities;

namespace Dominio.Interfaces.Repositories
{
    public interface IMotoRepository : IBaseRepository<Moto>
    {
        IEnumerable<Moto> GetByPlaca(string placa);
    }
}
