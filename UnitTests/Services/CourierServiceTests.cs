using Domain.Entities;
using Domain.Interfaces.Notification;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Storage;
using Domain.Services;
using Domain.Utilities;
using Microsoft.Extensions.Logging;
using Moq;
using UnitTests.Fixtures;
using UnitTests.Mocks;
using UnitTests.Mocks.Repositories;

namespace UnitTests.Services
{
    public class CourierServiceTests
    {

        private Mock<ICourierRepository> _courierRepositoryMock;
        private Mock<ILogger<CourierService>> _loggerMock;
        private Mock<INotificationContext> _notificationContextMock;
        private Mock<IStorage> _storage;

        public CourierServiceTests()
        {
            _courierRepositoryMock = CourierRepositoryMock.Create();
            _loggerMock = LoggerMock.Create<CourierService>();
            _notificationContextMock = NotificationContextMock.Create();
            _storage = new Mock<IStorage>();
            _storage.Setup(s => s.UploadFile(It.IsAny<Stream>(), It.IsAny<string>())).Returns((Stream stream, string keyName) => Task.FromResult("Url"+keyName));
        }

        [Fact]
        public void InsertCourierAsync_CnhAndCnpjAvailable_Insert()
        {
            // Arrange
            var courier = CourierFixture.Create();

            _courierRepositoryMock.SetupGet(new List<Courier>());
            _notificationContextMock.SetupHasNotifications(false);

            var service = new CourierService(_courierRepositoryMock.Object, _notificationContextMock.Object, _loggerMock.Object, _storage.Object);

            // Act
            service.InsertCourierAsync(courier).Wait();

            // Assert
            _courierRepositoryMock.Verify(repo => repo.InsertAsync(courier), Times.Once);
            _notificationContextMock.Verify(nc => nc.AddNotification(It.IsAny<string>()), Times.Never);
            _notificationContextMock.Verify(nc => nc.HasNotifications, Times.Once);
        }

        [Fact]
        public void InsertCourierAsync_CnhUnavailable_DontInsert()
        {
            // Arrange
            var courier = CourierFixture.Create();
            var courier2 = CourierFixture.Create();
            courier2.Cnh = courier.Cnh;

            _courierRepositoryMock.SetupGet(new List<Courier> { courier2 });
            _notificationContextMock.SetupHasNotifications(true);

            var service = new CourierService(_courierRepositoryMock.Object, _notificationContextMock.Object, _loggerMock.Object, _storage.Object);

            // Act
            service.InsertCourierAsync(courier).Wait();

            // Assert
            _courierRepositoryMock.Verify(repo => repo.InsertAsync(courier), Times.Never);
            _notificationContextMock.Verify(nc => nc.AddNotification("CNH já utilizada"), Times.Once);
            _notificationContextMock.Verify(nc => nc.HasNotifications, Times.Once);
        }

        [Fact]
        public void InsertCourierAsync_CnpjUnavailable_DontInsert()
        {
            // Arrange
            var courier = CourierFixture.Create();
            var courier2 = CourierFixture.Create();
            courier2.Cnpj = courier.Cnpj;

            _courierRepositoryMock.SetupGet(new List<Courier> { courier2 });
            _notificationContextMock.SetupHasNotifications(true);

            var service = new CourierService(_courierRepositoryMock.Object, _notificationContextMock.Object, _loggerMock.Object, _storage.Object);

            // Act
            service.InsertCourierAsync(courier).Wait();

            // Assert
            _courierRepositoryMock.Verify(repo => repo.InsertAsync(courier), Times.Never);
            _notificationContextMock.Verify(nc => nc.AddNotification("CNPJ já utilizado"), Times.Once);
            _notificationContextMock.Verify(nc => nc.HasNotifications, Times.Once);
        }

        [Fact]
        public async void UpdateCourierCnhImage_CourierFound_UpdateCnhImage()
        {
            // Arrange
            var courier = CourierFixture.Create();
            var oldUrl = courier.CnhImage;
            var image = new Domain.Utilities.File(
                default(Stream),
                "",
                "png"
            );

            _courierRepositoryMock.SetupGet(new List<Courier> { courier });

            var logLevel = LogLevel.Information;
            _loggerMock.SetupLogLevel(logLevel);

            var service = new CourierService(_courierRepositoryMock.Object, _notificationContextMock.Object, _loggerMock.Object, _storage.Object);

            // Act
            var result = await service.UpdateCourierCnhImage(courier.Id, image);

            // Assert
            _courierRepositoryMock.Verify(repo => repo.Update(courier), Times.Once);
            Assert.NotEqual(oldUrl, result.CnhImage);
        }

        [Fact]
        public async void UpdateCourierCnhImage_CourierNotFound_NotifyAndDontUpdate()
        {
            // Arrange
            var id = Guid.NewGuid();
            var imagem = new Domain.Utilities.File(
                    default(Stream),
                    "",
                    "png"
            );

            _courierRepositoryMock.SetupGet(new List<Courier>());

            var service = new CourierService(_courierRepositoryMock.Object, _notificationContextMock.Object, _loggerMock.Object, _storage.Object);

            // Act
            var result = await service.UpdateCourierCnhImage(id, imagem);

            // Assert
            _courierRepositoryMock.Verify(repo => repo.Update(It.IsAny<Courier>()), Times.Never);
            _notificationContextMock.Verify(nc => nc.AddNotification(ErrorNotifications.COURIER_NOT_FOUND), Times.Once);
            _loggerMock.VerifyNoOtherCalls();
            Assert.Null(result);
        }

        [Fact]
        public async void UpdateCourierCnhImage_InvalidImageType_NotifyAndDontUpdate()
        {
            // Arrange
            var courier = CourierFixture.Create();
            var imagem = new Domain.Utilities.File(
                    default(Stream),
                    "",
                    "jpeg"
            );

            _courierRepositoryMock.SetupGet(new List<Courier> { courier });

            var service = new CourierService(_courierRepositoryMock.Object, _notificationContextMock.Object, _loggerMock.Object, _storage.Object);

            // Act
            var result = await service.UpdateCourierCnhImage(courier.Id, imagem);

            // Assert
            _courierRepositoryMock.Verify(repo => repo.Update(It.IsAny<Courier>()), Times.Never);
            _notificationContextMock.Verify(nc => nc.AddNotification(ErrorNotifications.IMAGE_INVALID_TYPE), Times.Once);
            _loggerMock.VerifyNoOtherCalls();
            Assert.Null(result);
        }
    }
}
