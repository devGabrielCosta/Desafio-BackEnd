using Dominio.Entities;

namespace Dominio.Interfaces.Services
{
    public interface IAdminService
    {
        IEnumerable<Admin> GetAdmins();
        Task<Admin> CreateAdmin();
    }
}
