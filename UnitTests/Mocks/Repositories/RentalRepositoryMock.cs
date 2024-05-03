using Domain.Entities;
using Domain.Interfaces.Repositories;
using Moq;

namespace UnitTests.Mocks.Repositories
{
    public static class RentalRepositoryMock
    {
        public static Mock<IRentalRepository> Create()
        {
            return new Mock<IRentalRepository>();
        }
        public static void SetupGet(this Mock<IRentalRepository> mock, List<Rental> rentals)
        {
            mock.Setup(repo => repo.Get()).Returns(rentals.AsQueryable());
            mock.Setup(repo => repo.Get(It.IsAny<Guid>())).Returns((Guid id) => rentals.Where(p => p.Id == id).AsQueryable());
        }
        public static void SetupInsertAsync(this Mock<IRentalRepository> mock, Rental rental)
        {
            mock.Setup(repo => repo.InsertAsync(It.IsAny<Rental>())).Returns(Task.FromResult(rental));
        }
    }
}
