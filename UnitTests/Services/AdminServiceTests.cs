using Moq;
using Microsoft.Extensions.Logging;
using Dominio.Entities;
using Dominio.Services;
using UnitTests.Mocks;
using Dominio.Interfaces.Repositories;
using UnitTests.Mocks.Repositories;

namespace UnitTests.Services
{
    public class AdminServiceTests
    {
        private Mock<IAdminRepository> _adminRepositoryMock;
        private Mock<ILogger<AdminService>> _loggerMock;

        public AdminServiceTests()
        {
            _adminRepositoryMock = AdminRepositoryMock.Create();
            _loggerMock = LoggerMock.Create<AdminService>();
        }

        [Fact]
        public void Get_RetornaListaAdmins()
        {
            // Arrange
            var admins = new List<Admin>
                {
                    new Admin { },
                    new Admin { }
                };

            _adminRepositoryMock.SetupGet(admins);

            var adminService = new AdminService(_adminRepositoryMock.Object, _loggerMock.Object);

            // Act
            var result = adminService.Get();

            // Assert
            Assert.Equal(admins.Count, result.Count());
            Assert.Equal(admins.First().Id, result.First().Id);
            Assert.Equal(admins.Last().Id, result.Last().Id);
        }

        [Fact]
        public async Task CreateAdminAsync_InsereAdmin()
        {
            // Arrange
            var admin = new Admin { };

            _adminRepositoryMock.SetupGet(new List<Admin>());
            _adminRepositoryMock.SetupInsertAsync(admin);

            var logLevel = LogLevel.Information;
            _loggerMock.SetupLogLevel(logLevel);

            var adminService = new AdminService(_adminRepositoryMock.Object, _loggerMock.Object);

            // Act
            var result = await adminService.CreateAdminAsync();

            // Assert
            Assert.NotNull(result);
            _adminRepositoryMock.Verify(repo => repo.InsertAsync(It.IsAny<Admin>()), Times.Once);
        }
    }

}
