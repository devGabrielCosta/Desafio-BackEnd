using Moq;
using Dominio.Entities;
using Dominio.Handlers.Commands;
using Dominio.Handlers;
using UnitTests.Fixtures;
using Dominio.Interfaces.Repositories;
using UnitTests.Mocks.Repositories;

namespace UnitTests.Handlers
{
    public class NotificacaoHandlerTests
    {
        private Mock<IEntregadorRepository> _entregadorRepositoryMock;
        private Mock<IPedidoRepository> _pedidoRepositoryMock;

        public NotificacaoHandlerTests() 
        {
            _entregadorRepositoryMock = EntregadorRepositoryMock.Create();
            _pedidoRepositoryMock = PedidoRepositoryMock.Create();
        }

        [Fact]
        public void Handle_SemEntregadoresDisponiveis_ThrowsException()
        {
            // Arrange
            var pedidos = PedidoFixture.CreateList(1);

            _entregadorRepositoryMock.SetupEntregadoresAptoPedido(new List<Entregador>());
            _pedidoRepositoryMock.SetupGet(pedidos);

            var handler = new NotificacaoHandler(_entregadorRepositoryMock.Object, _pedidoRepositoryMock.Object);

            // Act
            Action act = () => handler.Handle(new NotificacaoCommand(Guid.NewGuid()));

            // Assert
            Assert.Throws<Exception>(act);
            _pedidoRepositoryMock.Verify(repo => repo.Update(It.IsAny<Pedido>()), Times.Never);
        }

        [Fact]
        public void Handle_PedidoNaoEncontrado_ThrowsArgumentException()
        {
            // Arrange
            var entregadores = EntregadorFixture.CreateList(1);

            _entregadorRepositoryMock.SetupEntregadoresAptoPedido(entregadores);
            _pedidoRepositoryMock.SetupGet(new List<Pedido>());

            var handler = new NotificacaoHandler(_entregadorRepositoryMock.Object, _pedidoRepositoryMock.Object);

            // Act
            Action act = () => handler.Handle(new NotificacaoCommand(Guid.NewGuid()));

            // Assert
            var exception = Assert.Throws<ArgumentException>(act);
            _pedidoRepositoryMock.Verify(repo => repo.Update(It.IsAny<Pedido>()), Times.Never);
        }

        [Fact]
        public void Handle_EntregadoresDisponiveisEPedidoEncontrado_UpdatesPedido()
        {
            // Arrange
            var entregadores = EntregadorFixture.CreateList(2);
            var pedidos = PedidoFixture.CreateList(1);

            var pedido = PedidoFixture.Create(Situacao.Disponivel);
            pedidos.Add(pedido);
            
            _entregadorRepositoryMock.SetupEntregadoresAptoPedido(entregadores);
            _pedidoRepositoryMock.SetupGet(pedidos);

            var handler = new NotificacaoHandler(_entregadorRepositoryMock.Object, _pedidoRepositoryMock.Object);

            // Act
            handler.Handle(new NotificacaoCommand(pedido.Id));

            // Assert
            foreach (var entregador in entregadores)
            {
                Assert.Contains(entregador, pedido.Notificados);
            }
            Assert.Equal(entregadores.Count, pedido.Notificados.Count);
            _pedidoRepositoryMock.Verify(repo => repo.Update(pedido), Times.Once);
        }
    }
}

