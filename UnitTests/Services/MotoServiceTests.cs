using Dominio.Entities;
using Dominio.Interfaces.Notification;
using Dominio.Interfaces.Repositories;
using Dominio.Services;
using Microsoft.Extensions.Logging;
using Moq;
using UnitTests.Fixtures;
using UnitTests.Mocks;
using UnitTests.Mocks.Repositories;

namespace UnitTests.Services
{
    public class MotoServiceTests
    {
        private readonly Mock<IMotoRepository> _motoRepositoryMock;
        private readonly Mock<INotificationContext> _notificationContextMock;
        private readonly Mock<ILogger<MotoService>> _loggerMock;

        public MotoServiceTests()
        {
            _motoRepositoryMock = MotoRepositoryMock.Create();
            _notificationContextMock = NotificationContextMock.Create();
            _loggerMock = LoggerMock.Create<MotoService>();
        }

        [Fact]
        public void GetByPlaca_PlacaValida_RetornaMoto()
        {
            // Arrange
            var motos = MotoFixture.CreateList(1);
            var placa = motos.First().Placa;

            _motoRepositoryMock.SetupGetByPlaca(motos);

            var motoService = new MotoService(_motoRepositoryMock.Object, _notificationContextMock.Object, _loggerMock.Object);

            // Act
            var result = motoService.GetByPlaca(placa);

            // Assert
            Assert.Equal(motos, result);
        }

        [Fact]
        public async Task InsertMoto_PlacaUnica_Successo()
        {
            // Arrange
            var moto = MotoFixture.Create();

            _motoRepositoryMock.SetupGetByPlaca(new List<Moto>());
            _motoRepositoryMock.SetupInsertAsync(moto);

            var motoService = new MotoService(_motoRepositoryMock.Object, _notificationContextMock.Object, _loggerMock.Object);

            // Act
            await motoService.InsertMotoAsync(moto);

            // Assert
            _notificationContextMock.Verify(nc => nc.AddNotification(It.IsAny<string>()), Times.Never);
            _motoRepositoryMock.Verify(mr => mr.InsertAsync(moto), Times.Once);
        }

        [Fact]
        public async Task InsertMoto_PlacaDuplicada_NotificaENaoInsere()
        {
            // Arrange
            var motos = MotoFixture.CreateList(2);
            var moto = MotoFixture.Create();

            moto.Placa = motos.First().Placa;

            _motoRepositoryMock.SetupGetByPlaca(motos);

            var motoService = new MotoService(_motoRepositoryMock.Object, _notificationContextMock.Object, _loggerMock.Object);

            // Act
            await motoService.InsertMotoAsync(moto);

            // Assert
            _notificationContextMock.Verify(nc => nc.AddNotification("Placa já utilizada"), Times.Once);
            _motoRepositoryMock.Verify(mr => mr.InsertAsync(moto), Times.Never);
        }

        [Fact]
        public void UpdatePlacaMoto_IdValidoEPlacaUnica_Successo()
        {
            // Arrange
            var moto = MotoFixture.Create();
            var placaNova = "ABC1234";

            _motoRepositoryMock.SetupGet(new List<Moto> { moto });
            _motoRepositoryMock.SetupGetByPlaca(new List<Moto>());

            var motoService = new MotoService(_motoRepositoryMock.Object, _notificationContextMock.Object, _loggerMock.Object);

            // Act
            var result = motoService.UpdatePlacaMoto(moto.Id, placaNova);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(placaNova, result.Placa);
            _notificationContextMock.Verify(nc => nc.AddNotification(It.IsAny<string>()), Times.Never);
            _motoRepositoryMock.Verify(mr => mr.Update(moto), Times.Once);
        }

        [Fact]
        public void UpdatePlacaMoto_IdInvalidoValidoEPlacaUnica_NotificaENaoAtualiza()
        {
            // Arrange
            var motos = MotoFixture.CreateList(2);
            var moto = MotoFixture.Create();

            _motoRepositoryMock.SetupGet(motos);
            _motoRepositoryMock.SetupGetByPlaca(motos);

            var motoService = new MotoService(_motoRepositoryMock.Object, _notificationContextMock.Object, _loggerMock.Object);

            // Act
            var result = motoService.UpdatePlacaMoto(moto.Id, moto.Placa);

            // Assert
            Assert.Null(result);
            _notificationContextMock.Verify(nc => nc.AddNotification("Moto não encontrada"), Times.Once);
            _motoRepositoryMock.Verify(mr => mr.Update(moto), Times.Never);
        }

        [Fact]
        public void UpdatePlacaMoto_IdValidoEPlacaDuplicada_NotificaENaoAtualiza()
        {
            // Arrange
            var motos = MotoFixture.CreateList(2);
            var moto = MotoFixture.Create();

            moto.Placa = motos.First().Placa;

            motos.Add(moto);

            _motoRepositoryMock.SetupGet(motos);
            _motoRepositoryMock.SetupGetByPlaca(motos);

            var motoService = new MotoService(_motoRepositoryMock.Object, _notificationContextMock.Object, _loggerMock.Object);

            // Act
            var result = motoService.UpdatePlacaMoto(moto.Id, moto.Placa);

            // Assert
            Assert.Null(result);
            _notificationContextMock.Verify(nc => nc.AddNotification("Placa já utilizada"), Times.Once);
            _motoRepositoryMock.Verify(mr => mr.Update(moto), Times.Never);
        }

        [Fact]
        public void UpdateMoto_MotoValida_Sucesso()
        {
            // Arrange
            var motos = MotoFixture.CreateList(1);
            var moto = motos.First();

            _motoRepositoryMock.SetupGet(motos);

            var motoService = new MotoService(_motoRepositoryMock.Object, _notificationContextMock.Object, _loggerMock.Object);

            // Act
            motoService.UpdateMoto(moto);

            // Assert
            _notificationContextMock.Verify(nc => nc.AddNotification(It.IsAny<string>()), Times.Never);
            _motoRepositoryMock.Verify(mr => mr.Update(moto), Times.Once);
        }

        [Fact]
        public void DeleteMoto_IdValidoESemLocacoes_Successo()
        {
            // Arrange
            var motos = MotoFixture.CreateList(1);
            var moto = motos.First();

            _motoRepositoryMock.SetupGetLocacoes(motos);

            var motoService = new MotoService(_motoRepositoryMock.Object, _notificationContextMock.Object, _loggerMock.Object);

            // Act
            motoService.DeleteMoto(moto.Id);

            // Assert
            _notificationContextMock.Verify(nc => nc.AddNotification(It.IsAny<string>()), Times.Never);
            _motoRepositoryMock.Verify(mr => mr.Delete(moto.Id), Times.Once);
        }

        [Fact]
        public void DeleteMoto_IdValidoEComLocacoes_NotificaENaoDeleta()
        {
            // Arrange
            var moto = MotoFixture.Create();
            moto.Locacoes.Add(LocacaoFixture.Create());
            var motos = MotoFixture.CreateList(1);
            motos.Add(moto);

            _motoRepositoryMock.SetupGetLocacoes(motos);

            var motoService = new MotoService(_motoRepositoryMock.Object, _notificationContextMock.Object, _loggerMock.Object);

            // Act
            motoService.DeleteMoto(moto.Id);

            // Assert
            _notificationContextMock.Verify(nc => nc.AddNotification("Moto possui locação ativa"), Times.Once);
            _motoRepositoryMock.Verify(mr => mr.Delete(It.IsAny<Guid>()), Times.Never);
        }

        [Fact]
        public void DeleteMoto_IdInvalido_NotificaENaoDeleta()
        {
            // Arrange
            var motoId = Guid.NewGuid();

            _motoRepositoryMock.SetupGet(new List<Moto>());

            var motoService = new MotoService(_motoRepositoryMock.Object, _notificationContextMock.Object, _loggerMock.Object);

            // Act
            motoService.DeleteMoto(motoId);

            // Assert
            _notificationContextMock.Verify(nc => nc.AddNotification("Moto não encontrada"), Times.Once);
            _motoRepositoryMock.Verify(mr => mr.Delete(It.IsAny<Guid>()), Times.Never);
        }

    }
}
