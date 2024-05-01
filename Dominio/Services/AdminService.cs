using Dominio.Entities;
using Dominio.Interfaces.Repositories;
using Dominio.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace Dominio.Services
{
    public class AdminService : IAdminService
    {
        private IAdminRepository _repository { get; }
        private ILogger _logger { get; }

        public AdminService(IAdminRepository repository, ILogger<AdminService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public IEnumerable<Admin> Get()
        {
            return _repository.Get().ToList();
        }

        public async Task<Admin> CreateAdminAsync()
        {
            var admin = new Admin();
            await _repository.InsertAsync(admin);

            _logger.LogInformation($"Novo administrador registrado. Id: {admin.Id}");

            return admin;
        }
    }
}
