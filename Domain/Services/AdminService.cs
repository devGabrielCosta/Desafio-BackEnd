using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace Domain.Services
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

            _logger.LogInformation($"New admin registered. Id: {admin.Id}");

            return admin;
        }
    }
}
