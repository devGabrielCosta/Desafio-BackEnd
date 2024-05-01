using Dominio.Entities;
using Dominio.Handlers.Commands;
using Dominio.Interfaces.Mensageria;
using Dominio.Interfaces.Notification;
using Dominio.Interfaces.Repositories;
using Dominio.Interfaces.Services;
using Dominio.Services;
using Microsoft.Extensions.Logging;
using Moq;
using UnitTests.Fixtures;
using UnitTests.Mocks.Repositories;
using UnitTests.Mocks.Services;
using UnitTests.Mocks;

namespace UnitTests.Services
{
    public class PedidoServiceTests
    {
        private readonly Mock<IPedidoRepository> _pedidoRepositoryMock;
        private readonly Mock<IPublisher<NotificacaoCommand>> _publisherNotificacaoMock;
        private readonly Mock<IEntregadorService> _entregadorServiceMock;
        private readonly Mock<INotificationContext> _notificationContextMock;
        private readonly Mock<ILogger<PedidoService>> _loggerMock;

        public PedidoServiceTests()
        {
            _pedidoRepositoryMock = PedidoRepositoryMock.Create();
            _publisherNotificacaoMock = new Mock<IPublisher<NotificacaoCommand>>();
            _publisherNotificacaoMock.Setup(x => x.Publish(It.IsAny<NotificacaoCommand>()));
            _entregadorServiceMock = EntregadorServiceMock.Create();
            _notificationContextMock = NotificationContextMock.Create();
            _loggerMock = LoggerMock.Create<PedidoService>();
        }

        [Fact]
        public void GetNotificados_PedidoExistente_RetornaPedido()
        {
            // Arrange
            var pedidos = PedidoFixture.CreateList(2);
            var pedido = pedidos.First();

            _pedidoRepositoryMock.SetupGetNotificados(pedidos);

            var pedidoService = new PedidoService(
                _pedidoRepositoryMock.Object,
                _publisherNotificacaoMock.Object,
                _entregadorServiceMock.Object,
                _notificationContextMock.Object,
                _loggerMock.Object);

            // Act
            var result = pedidoService.GetNotificados(pedido.Id);

            // Assert
            Assert.Equal(pedido, result);
        }

        [Fact]
        public async Task InsertPedidoAsync_PedidoInserido_Sucesso()
        {
            // Arrange
            var pedido = PedidoFixture.Create();

            _pedidoRepositoryMock.SetupInsertAsync(pedido);

            var pedidoService = new PedidoService(
                _pedidoRepositoryMock.Object,
                _publisherNotificacaoMock.Object,
                _entregadorServiceMock.Object,
                _notificationContextMock.Object,
                _loggerMock.Object);

            // Act
            var result = await pedidoService.InsertPedidoAsync(pedido);

            // Assert
            _publisherNotificacaoMock.Verify(publisher => publisher.Publish(It.IsAny<NotificacaoCommand>()), Times.Once);
            Assert.Equal(pedido, result);
        }

        [Fact]
        public void AceitarPedido_PedidoEEntregadorValidos_Sucesso()
        {
            // Arrange
            var entregadores = EntregadorFixture.CreateList(1);
            var entregador = entregadores.First();

            var pedido = PedidoFixture.Create(Situacao.Disponivel);
            pedido.Notificados.Add(entregador);
            var pedidos = PedidoFixture.CreateList(1);
            pedidos.Add(pedido);
            
            _pedidoRepositoryMock.SetupGetNotificados(pedidos);
            _entregadorServiceMock.SetupGet(entregadores);

            var pedidoService = new PedidoService(
                _pedidoRepositoryMock.Object,
                _publisherNotificacaoMock.Object,
                _entregadorServiceMock.Object,
                _notificationContextMock.Object,
                _loggerMock.Object);

            // Act
            pedidoService.AceitarPedido(pedido.Id, entregador.Id);

            // Assert
            _pedidoRepositoryMock.Verify(repo => repo.Update(pedido), Times.Once);
            Assert.Equal(Situacao.Aceito, pedido.Situacao);
            Assert.Equal(entregador, pedido.Entregador);
        }

        [Fact]
        public void AceitarPedido_EntregadorNaoEncontrado_NotificaENaoAtualiza()
        {
            // Arrange
            var pedido = PedidoFixture.Create(Situacao.Disponivel);
            var pedidos = PedidoFixture.CreateList(1);
            pedidos.Add(pedido);

            var entregadores = EntregadorFixture.CreateList(1);
            var entregador = EntregadorFixture.Create();

            _pedidoRepositoryMock.SetupGetNotificados(pedidos);
            _entregadorServiceMock.SetupGet(entregadores);

            var pedidoService = new PedidoService(
                _pedidoRepositoryMock.Object,
                _publisherNotificacaoMock.Object,
                _entregadorServiceMock.Object,
                _notificationContextMock.Object,
                _loggerMock.Object);

            // Act
            pedidoService.AceitarPedido(pedido.Id, entregador.Id);

            // Assert
            _pedidoRepositoryMock.Verify(repo => repo.Update(pedido), Times.Never);
            _notificationContextMock.Verify(nc => nc.AddNotification("Entregador não encontrado"), Times.Once);
        }

        [Fact]
        public void AceitarPedido_PedidoNaoEncontrado_NotificaENaoAtualiza()
        {
            // Arrange
            var pedido = PedidoFixture.Create();
            var pedidos = PedidoFixture.CreateList(1);

            var entregadores = EntregadorFixture.CreateList(1);
            var entregador = entregadores.First();

            _pedidoRepositoryMock.SetupGetNotificados(pedidos);
            _entregadorServiceMock.SetupGet(entregadores);

            var pedidoService = new PedidoService(
                _pedidoRepositoryMock.Object,
                _publisherNotificacaoMock.Object,
                _entregadorServiceMock.Object,
                _notificationContextMock.Object,
                _loggerMock.Object);

            // Act
            pedidoService.AceitarPedido(pedido.Id, entregador.Id);

            // Assert
            _pedidoRepositoryMock.Verify(repo => repo.Update(It.IsAny<Pedido>()), Times.Never);
            _notificationContextMock.Verify(nc => nc.AddNotification("Pedido não encontrado"), Times.Once);
        }

        [Fact]
        public void AceitarPedido_EntregadorNaoRecebeuNotificacao_NotificaENaoAtualiza()
        {
            // Arrange
            var pedido = PedidoFixture.Create();
            var pedidos = PedidoFixture.CreateList(1);
            pedidos.Add(pedido);

            var entregadores = EntregadorFixture.CreateList(1);
            var entregador = EntregadorFixture.Create();
            entregadores.Add(entregador);

            _pedidoRepositoryMock.SetupGetNotificados(pedidos);
            _entregadorServiceMock.SetupGet(entregadores);

            var pedidoService = new PedidoService(
                _pedidoRepositoryMock.Object,
                _publisherNotificacaoMock.Object,
                _entregadorServiceMock.Object,
                _notificationContextMock.Object,
                _loggerMock.Object);

            // Act
            pedidoService.AceitarPedido(pedido.Id, entregador.Id);

            // Assert
            _pedidoRepositoryMock.Verify(repo => repo.Update(It.IsAny<Pedido>()), Times.Never);
            _notificationContextMock.Verify(nc => nc.AddNotification("Entregador não recebeu notificação"), Times.Once);
        }

        [Fact]
        public void AceitarPedido_PedidoNaoEstaDisponivel_NotificaENaoAtualiza()
        {
            // Arrange
            var entregadores = EntregadorFixture.CreateList(1);
            var entregador = EntregadorFixture.Create();
            entregadores.Add(entregador);

            var pedido = PedidoFixture.Create(Situacao.Aceito);
            pedido.Notificados.Add(entregador);
            var pedidos = PedidoFixture.CreateList(1);
            pedidos.Add(pedido);

            _pedidoRepositoryMock.SetupGetNotificados(pedidos);
            _entregadorServiceMock.SetupGet(entregadores);

            var pedidoService = new PedidoService(
                _pedidoRepositoryMock.Object,
                _publisherNotificacaoMock.Object,
                _entregadorServiceMock.Object,
                _notificationContextMock.Object,
                _loggerMock.Object);

            // Act
            pedidoService.AceitarPedido(pedido.Id, entregador.Id);

            // Assert
            _pedidoRepositoryMock.Verify(repo => repo.Update(It.IsAny<Pedido>()), Times.Never);
            _notificationContextMock.Verify(nc => nc.AddNotification("Pedido não está com status Disponivel"), Times.Once);
        }

        [Fact]
        public void FinalizarPedido_PedidoEEntregadorValidos_Sucesso()
        {
            // Arrange
            var entregador = EntregadorFixture.Create();

            var pedido = PedidoFixture.Create(Situacao.Aceito);
            pedido.EntregadorId = entregador.Id;
            var pedidos = PedidoFixture.CreateList(1);
            pedidos.Add(pedido);

            _pedidoRepositoryMock.SetupGet(pedidos);

            var logLevel = LogLevel.Information;
            _loggerMock.SetupLogLevel(logLevel);

            var pedidoService = new PedidoService(
                _pedidoRepositoryMock.Object,
                _publisherNotificacaoMock.Object,
                _entregadorServiceMock.Object,
                _notificationContextMock.Object,
                _loggerMock.Object);

            // Act
            pedidoService.FinalizarPedido(pedido.Id, entregador.Id);

            // Assert
            _pedidoRepositoryMock.Verify(repo => repo.Update(pedido), Times.Once);
            Assert.Equal(Situacao.Entregue, pedido.Situacao);
        }

        [Fact]
        public void FinalizarPedido_PedidoNaoPertenceAoEntregador_NotificaENaoAtualiza()
        {
            // Arrange
            var entregador = EntregadorFixture.Create();

            var pedido = PedidoFixture.Create(Situacao.Aceito);
            var pedidos = PedidoFixture.CreateList(1);
            pedidos.Add(pedido);

            _pedidoRepositoryMock.SetupGet(pedidos);

            var pedidoService = new PedidoService(
                _pedidoRepositoryMock.Object,
                _publisherNotificacaoMock.Object,
                _entregadorServiceMock.Object,
                _notificationContextMock.Object,
                _loggerMock.Object);

            // Act
            pedidoService.FinalizarPedido(pedido.Id, entregador.Id);

            // Assert
            _pedidoRepositoryMock.Verify(repo => repo.Update(It.IsAny<Pedido>()), Times.Never);
            _notificationContextMock.Verify(nc => nc.AddNotification("Pedido não pertence ao entregador"), Times.Once);
        }

        [Fact]
        public void FinalizarPedido_PedidoNaoFoiAceito_NotificaENaoAtualiza()
        {
            // Arrange
            var entregador = EntregadorFixture.Create();

            var pedido = PedidoFixture.Create(Situacao.Disponivel);
            pedido.EntregadorId = entregador.Id;
            var pedidos = PedidoFixture.CreateList(1);
            pedidos.Add(pedido);

            _pedidoRepositoryMock.SetupGet(pedidos);

            var pedidoService = new PedidoService(
                _pedidoRepositoryMock.Object,
                _publisherNotificacaoMock.Object,
                _entregadorServiceMock.Object,
                _notificationContextMock.Object,
                _loggerMock.Object);

            // Act
            pedidoService.FinalizarPedido(pedido.Id, entregador.Id);

            // Assert
            _pedidoRepositoryMock.Verify(repo => repo.Update(It.IsAny<Pedido>()), Times.Never);
            _notificationContextMock.Verify(nc => nc.AddNotification("Pedido ainda não foi aceito ou já foi entregue"), Times.Once);
        }
    }
}
