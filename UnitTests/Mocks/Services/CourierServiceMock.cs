using Domain.Entities;
using Domain.Interfaces.Services;
using Moq;

namespace UnitTests.Mocks.Services
{
    public static class CourierServiceMock
    {
        public static Mock<ICourierService> Create()
        {
            return new Mock<ICourierService>();
        }

        public static void SetupGet(this Mock<ICourierService> mock, List<Courier> couriers)
        {
            mock.Setup(repo => repo.Get(It.IsAny<Guid>())).Returns((Guid id) => couriers.FirstOrDefault(p => p.Id == id));
        }

        public static void SetupGetRentals(this Mock<ICourierService> mock, List<Courier> couriers)
        {
            mock.Setup(repo => repo.GetRentals(It.IsAny<Guid>())).Returns((Guid id) => couriers.FirstOrDefault(p => p.Id == id));
        }
    }
}
