using Dominio.Entities;

namespace Dominio.Interfaces.Services
{
    public interface IMotoService
    {
        IEnumerable<Moto> GetByPlaca(string placa);
        IEnumerable<Moto> GetMotosDisponiveis();
        Task InsertMotoAsync(Moto moto);
        Moto UpdatePlacaMoto(Guid id, string placa);
        void UpdateMoto(Moto moto);
        void DeleteMoto(Guid id);

    }
}
