using Dominio.Entities;

namespace Dominio.Interfaces.Services
{
    public interface IMotoService
    {
        IEnumerable<Moto> GetByPlaca(string placa);
        Task<Moto> InsertMoto(Moto moto);
        Task<Moto> UpdatePlacaMoto(Guid id, string placa);
        Task DeleteMoto(Guid id);

    }
}
