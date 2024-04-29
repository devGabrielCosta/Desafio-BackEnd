using Dominio.Entities;
using Dominio.Interfaces.Repositories;
using Dominio.Interfaces.Services;

namespace Dominio.Services
{
    public class AdminService : IAdminService
    {
        private IAdminRepository _repository { get; }

        public AdminService(IAdminRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Admin> GetAdmins()
        {
            return _repository.Get().ToList();
        }

        public Admin CreateAdmin()
        {
            var admin = new Admin();
            _repository.InsertAsync(admin);
            return admin;
        }
    }
}
