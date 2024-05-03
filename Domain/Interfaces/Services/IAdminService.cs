using Domain.Entities;

namespace Domain.Interfaces.Services
{
    public interface IAdminService
    {
        IEnumerable<Admin> Get();
        Task<Admin> CreateAdminAsync();
    }
}
