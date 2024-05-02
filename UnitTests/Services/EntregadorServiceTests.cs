using Dominio.Entities;
using Dominio.Interfaces.Notification;
using Dominio.Interfaces.Repositories;
using Dominio.Interfaces.Storage;
using Dominio.Services;
using Dominio.Utilities;
using Microsoft.Extensions.Logging;
using Moq;
using UnitTests.Fixtures;
using UnitTests.Mocks;
using UnitTests.Mocks.Repositories;

namespace UnitTests.Services
{
    public class EntregadorServiceTests
    {

        private Mock<IEntregadorRepository> _entregadorRepositoryMock;
        private Mock<ILogger<EntregadorService>> _loggerMock;
        private Mock<INotificationContext> _notificationContextMock;
        private Mock<IStorage> _storage;

        public EntregadorServiceTests()
        {
            _entregadorRepositoryMock = EntregadorRepositoryMock.Create();
            _loggerMock = LoggerMock.Create<EntregadorService>();
            _notificationContextMock = NotificationContextMock.Create();
            _storage = new Mock<IStorage>();
            _storage.Setup(s => s.UploadFile(It.IsAny<Stream>(), It.IsAny<string>())).Returns((Stream stream, string keyName) => Task.FromResult("Url"+keyName));
        }

        [Fact]
        public void InsertEntregadorAsync_CnhECnpjNaoUtilizados_InsereEntregador()
        {
            // Arrange
            var entregador = EntregadorFixture.Create();

            _entregadorRepositoryMock.SetupGet(new List<Entregador>());
            _notificationContextMock.SetupHasNotifications(false);

            var service = new EntregadorService(_entregadorRepositoryMock.Object, _notificationContextMock.Object, _loggerMock.Object, _storage.Object);

            // Act
            service.InsertEntregadorAsync(entregador).Wait();

            // Assert
            _entregadorRepositoryMock.Verify(repo => repo.InsertAsync(entregador), Times.Once);
            _notificationContextMock.Verify(nc => nc.AddNotification(It.IsAny<string>()), Times.Never);
            _notificationContextMock.Verify(nc => nc.HasNotifications, Times.Once);
        }

        [Fact]
        public void InsertEntregadorAsync_CnhJaUsado_NaoInsereEntregador()
        {
            // Arrange
            var entregador = EntregadorFixture.Create();
            var entregador2 = EntregadorFixture.Create();
            entregador2.Cnh = entregador.Cnh;

            _entregadorRepositoryMock.SetupGet(new List<Entregador> { entregador2 });
            _notificationContextMock.SetupHasNotifications(true);

            var service = new EntregadorService(_entregadorRepositoryMock.Object, _notificationContextMock.Object, _loggerMock.Object, _storage.Object);

            // Act
            service.InsertEntregadorAsync(entregador).Wait();

            // Assert
            _entregadorRepositoryMock.Verify(repo => repo.InsertAsync(entregador), Times.Never);
            _notificationContextMock.Verify(nc => nc.AddNotification("CNH já utilizada"), Times.Once);
            _notificationContextMock.Verify(nc => nc.HasNotifications, Times.Once);
        }

        [Fact]
        public void InsertEntregadorAsync_CnpjJaUsado_NaoInsereEntregador()
        {
            // Arrange
            var entregador = EntregadorFixture.Create();
            var entregador2 = EntregadorFixture.Create();
            entregador2.Cnpj = entregador.Cnpj;

            _entregadorRepositoryMock.SetupGet(new List<Entregador> { entregador2 });
            _notificationContextMock.SetupHasNotifications(true);

            var service = new EntregadorService(_entregadorRepositoryMock.Object, _notificationContextMock.Object, _loggerMock.Object, _storage.Object);

            // Act
            service.InsertEntregadorAsync(entregador).Wait();

            // Assert
            _entregadorRepositoryMock.Verify(repo => repo.InsertAsync(entregador), Times.Never);
            _notificationContextMock.Verify(nc => nc.AddNotification("CNPJ já utilizado"), Times.Once);
            _notificationContextMock.Verify(nc => nc.HasNotifications, Times.Once);
        }

        [Fact]
        public async void UpdateCnhImagemEntregador_EntregadorExiste_AtualizaCnhImagem()
        {
            // Arrange
            var entregador = EntregadorFixture.Create();
            var antigaUrl = entregador.CnhImagem;
            var imagem = new Dominio.Utilities.File(
                    default(Stream),
                    "",
                    "png"
            );

            _entregadorRepositoryMock.SetupGet(new List<Entregador> { entregador });

            var logLevel = LogLevel.Information;
            _loggerMock.SetupLogLevel(logLevel);

            var service = new EntregadorService(_entregadorRepositoryMock.Object, _notificationContextMock.Object, _loggerMock.Object, _storage.Object);

            // Act
            var result = await service.UpdateCnhImagemEntregador(entregador.Id, imagem);

            // Assert
            _entregadorRepositoryMock.Verify(repo => repo.Update(entregador), Times.Once);
            Assert.NotEqual(antigaUrl, result.CnhImagem);
        }

        [Fact]
        public async void UpdateCnhImagemEntregador_EntregadorNaoEncontrado_NotificaENaoAtualiza()
        {
            // Arrange
            var id = Guid.NewGuid();
            var imagem = new Dominio.Utilities.File(
                    default(Stream),
                    "",
                    "png"
            );

            _entregadorRepositoryMock.SetupGet(new List<Entregador>());

            var service = new EntregadorService(_entregadorRepositoryMock.Object, _notificationContextMock.Object, _loggerMock.Object, _storage.Object);

            // Act
            var result = await service.UpdateCnhImagemEntregador(id, imagem);

            // Assert
            _entregadorRepositoryMock.Verify(repo => repo.Update(It.IsAny<Entregador>()), Times.Never);
            _notificationContextMock.Verify(nc => nc.AddNotification(ErrorNotifications.ENTREGADOR_NAO_ENCONTRADO), Times.Once);
            _loggerMock.VerifyNoOtherCalls();
            Assert.Null(result);
        }

        [Fact]
        public async void UpdateCnhImagemEntregador_TipoDeImagemInvalid_NotificaENaoAtualiza()
        {
            // Arrange
            var entregador = EntregadorFixture.Create();
            var imagem = new Dominio.Utilities.File(
                    default(Stream),
                    "",
                    "jpeg"
            );

            _entregadorRepositoryMock.SetupGet(new List<Entregador> { entregador });

            var service = new EntregadorService(_entregadorRepositoryMock.Object, _notificationContextMock.Object, _loggerMock.Object, _storage.Object);

            // Act
            var result = await service.UpdateCnhImagemEntregador(entregador.Id, imagem);

            // Assert
            _entregadorRepositoryMock.Verify(repo => repo.Update(It.IsAny<Entregador>()), Times.Never);
            _notificationContextMock.Verify(nc => nc.AddNotification(ErrorNotifications.IMAGEM_FORMATO_INVALIDO), Times.Once);
            _loggerMock.VerifyNoOtherCalls();
            Assert.Null(result);
        }
    }
}
