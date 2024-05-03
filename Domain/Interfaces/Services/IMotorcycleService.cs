using Domain.Entities;

namespace Domain.Interfaces.Services
{
    public interface IMotorcycleService
    {
        IEnumerable<Motorcycle> GetByLicensePlate(string licensePlate);
        IEnumerable<Motorcycle> GetAvailable();
        Task InsertMotorcycleAsync(Motorcycle motorcycle);
        Motorcycle UpdateMotorcycleLicensePlate(Guid id, string licensePlate);
        void UpdateMotorcycle(Motorcycle moto);
        void DeleteMotorcycle(Guid id);

    }
}
