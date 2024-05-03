using Domain.Entities;
using Domain.Interfaces.Repositories;
using Moq;

namespace UnitTests.Mocks.Repositories
{

    public static class AdminRepositoryMock
    {
        public static Mock<IAdminRepository> Create()
        {
            return new Mock<IAdminRepository>();
        }

        public static void SetupGet(this Mock<IAdminRepository> mock, List<Admin> admins)
        {
            mock.Setup(repo => repo.Get()).Returns(admins.AsQueryable());
        }

        public static void SetupInsertAsync(this Mock<IAdminRepository> mock, Admin admin)
        {
            mock.Setup(repo => repo.InsertAsync(It.IsAny<Admin>())).Returns(Task.FromResult(admin));
        }
    }
}
