using Domain.Entities;
using Domain.Interfaces.Services;
using Moq;

namespace UnitTests.Mocks.Services
{
    public static class MotorcycleServiceMock
    {
        public static Mock<IMotorcycleService> Create()
        {
            return new Mock<IMotorcycleService>();
        }

        public static void SetupGetMotosDisponiveis(this Mock<IMotorcycleService> mock, List<Motorcycle> motos)
        {
            mock.Setup(repo => repo.GetAvailable()).Returns(motos.AsQueryable());
        }
    }
}
