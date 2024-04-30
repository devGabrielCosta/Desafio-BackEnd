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

        public IEnumerable<Admin> Get()
        {
            return _repository.Get().ToList();
        }

        public async Task<Admin> CreateAdminAsync()
        {
            var admin = new Admin();
            await _repository.InsertAsync(admin);
            return admin;
        }
    }
}
