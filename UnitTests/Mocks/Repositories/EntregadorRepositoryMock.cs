using Dominio.Entities;
using Dominio.Interfaces.Repositories;
using Moq;

namespace UnitTests.Mocks.Repositories
{
    public static class EntregadorRepositoryMock
    {
        public static Mock<IEntregadorRepository> Create()
        {
            return new Mock<IEntregadorRepository>();
        }

        public static void SetupEntregadoresAptoPedido(this Mock<IEntregadorRepository> mock, List<Entregador> entregadores)
        {
            mock.Setup(repo => repo.EntregadoresAptosPedido()).Returns(entregadores.AsQueryable());
        }

        public static void SetupGet(this Mock<IEntregadorRepository> mock, List<Entregador> entregadores)
        {
            mock.Setup(repo => repo.Get()).Returns(entregadores.AsQueryable());
            mock.Setup(repo => repo.Get(It.IsAny<Guid>())).Returns((Guid id) => entregadores.Where(p => p.Id == id).AsQueryable());
        }
    }
}