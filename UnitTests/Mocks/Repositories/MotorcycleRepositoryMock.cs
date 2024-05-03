using Domain.Entities;
using Domain.Interfaces.Repositories;
using Moq;

namespace UnitTests.Mocks.Repositories
{
    public static class MotorcycleRepositoryMock
    {
        public static Mock<IMotoRepository> Create()
        {
            return new Mock<IMotoRepository>();
        }
        public static void SetupGet(this Mock<IMotoRepository> mock, List<Motorcycle> motorcycles)
        {
            mock.Setup(repo => repo.Get(It.IsAny<Guid>())).Returns((Guid id) => motorcycles.Where(p => p.Id == id).AsQueryable());
        }

        public static void SetupGetByLicensePlate(this Mock<IMotoRepository> mock, List<Motorcycle> motorcycles)
        {
            mock.Setup(repo => repo.GetByLicensePlate(It.IsAny<string>())).Returns((string licensePlate) => motorcycles.Where(p => p.LicensePlate.Equals(licensePlate)).AsQueryable());
        }

        public static void SetupGetLocacoes(this Mock<IMotoRepository> mock, List<Motorcycle> motorcycles)
        {
            mock.Setup(repo => repo.GetRentals()).Returns(motorcycles.AsQueryable());
        }

        public static void SetupInsertAsync(this Mock<IMotoRepository> mock, Motorcycle motorcycle)
        {
            mock.Setup(repo => repo.InsertAsync(It.IsAny<Motorcycle>())).Returns(Task.FromResult(motorcycle));
        }
    }
}