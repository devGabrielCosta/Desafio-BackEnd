using Dominio.Entities;
using Dominio.Interfaces.Repositories;
using Moq;

namespace UnitTests.Mocks.Repositories
{
    public static class PedidoRepositoryMock
    {
        public static Mock<IPedidoRepository> Create()
        {
            return new Mock<IPedidoRepository>();
        }

        public static void SetupGet(this Mock<IPedidoRepository> mock, List<Pedido> pedidos)
        {
            mock.Setup(repo => repo.Get(It.IsAny<Guid>())).Returns((Guid id) => pedidos.Where(p => p.Id == id).AsQueryable());
        }

        public static void SetupGetNotificados(this Mock<IPedidoRepository> mock, List<Pedido> pedidos)
        {
            mock.Setup(repo => repo.GetNotificados(It.IsAny<Guid>())).Returns((Guid id) => pedidos.Where(p => p.Id == id).AsQueryable());
        }

        public static Mock<IPedidoRepository> GetPedidoRepositoryMock(List<Pedido> pedidos)
        {
            var pedidoRepositoryMock = new Mock<IPedidoRepository>();

            pedidoRepositoryMock.Setup(repo => repo.Get(It.IsAny<Guid>())).Returns((Guid id) => pedidos.Where(p => p.Id == id).AsQueryable());

            return pedidoRepositoryMock;
        }

        public static void SetupInsertAsync(this Mock<IPedidoRepository> mock, Pedido pedido)
        {
            mock.Setup(repo => repo.InsertAsync(It.IsAny<Pedido>())).Returns(Task.FromResult(pedido));
        }
    }
}
