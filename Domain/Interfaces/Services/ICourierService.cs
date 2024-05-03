using Domain.Entities;

namespace Domain.Interfaces.Services
{
    public interface ICourierService
    {
        IEnumerable<Courier> Get();
        Courier Get(Guid Id);
        Courier GetRentals(Guid Id);
        Task InsertCourierAsync(Courier courier);
        Task<Courier> UpdateCourierCnhImage(Guid id, Utilities.File uploadImage);
    }
}
