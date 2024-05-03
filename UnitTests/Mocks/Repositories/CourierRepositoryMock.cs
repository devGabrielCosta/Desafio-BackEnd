using Domain.Entities;
using Domain.Interfaces.Repositories;
using Moq;

namespace UnitTests.Mocks.Repositories
{
    public static class CourierRepositoryMock
    {
        public static Mock<ICourierRepository> Create()
        {
            return new Mock<ICourierRepository>();
        }

        public static void SetupAvailableCouriersForOrder(this Mock<ICourierRepository> mock, List<Courier> couriers)
        {
            mock.Setup(repo => repo.AvailableCouriersForOrder()).Returns(couriers.AsQueryable());
        }

        public static void SetupGet(this Mock<ICourierRepository> mock, List<Courier> couriers)
        {
            mock.Setup(repo => repo.Get()).Returns(couriers.AsQueryable());
            mock.Setup(repo => repo.Get(It.IsAny<Guid>())).Returns((Guid id) => couriers.Where(p => p.Id == id).AsQueryable());
        }
    }
}