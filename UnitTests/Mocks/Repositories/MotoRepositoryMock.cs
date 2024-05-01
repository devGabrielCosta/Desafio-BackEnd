using Dominio.Entities;
using Dominio.Interfaces.Repositories;
using Moq;

namespace UnitTests.Mocks.Repositories
{
    public static class MotoRepositoryMock
    {
        public static Mock<IMotoRepository> Create()
        {
            return new Mock<IMotoRepository>();
        }
        public static void SetupGet(this Mock<IMotoRepository> mock, List<Moto> motos)
        {
            mock.Setup(repo => repo.Get(It.IsAny<Guid>())).Returns((Guid id) => motos.Where(p => p.Id == id).AsQueryable());
        }

        public static void SetupGetByPlaca(this Mock<IMotoRepository> mock, List<Moto> motos)
        {
            mock.Setup(repo => repo.GetByPlaca(It.IsAny<string>())).Returns((string placa) => motos.Where(p => p.Placa.Equals(placa)).AsQueryable());
        }

        public static void SetupGetLocacoes(this Mock<IMotoRepository> mock, List<Moto> motos)
        {
            mock.Setup(repo => repo.GetLocacoes()).Returns(motos.AsQueryable());
        }

        public static void SetupInsertAsync(this Mock<IMotoRepository> mock, Moto moto)
        {
            mock.Setup(repo => repo.InsertAsync(It.IsAny<Moto>())).Returns(Task.FromResult(moto));
        }
    }
}