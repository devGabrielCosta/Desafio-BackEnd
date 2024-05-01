using Dominio.Entities;
using Dominio.Interfaces.Services;
using Moq;

namespace UnitTests.Mocks.Services
{
    public static class MotoServiceMock
    {
        public static Mock<IMotoService> Create()
        {
            return new Mock<IMotoService>();
        }

        public static void SetupGetMotosDisponiveis(this Mock<IMotoService> mock, List<Moto> motos)
        {
            mock.Setup(repo => repo.GetMotosDisponiveis()).Returns(motos.AsQueryable());
        }
    }
}
