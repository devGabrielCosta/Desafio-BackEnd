using Dominio.Entities;

namespace Dominio.Interfaces.Services
{
    public interface IMotoService
    {
        IEnumerable<Moto> GetByPlaca(string placa);
        IEnumerable<Moto> GetMotosDisponiveis();
        Task<Moto> InsertMoto(Moto moto);
        Task<Moto> UpdatePlacaMoto(Guid id, string placa);
        Task UpdateMoto(Moto moto);
        Task DeleteMoto(Guid id);

    }
}
