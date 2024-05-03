using Domain.Entities;
using Domain.Interfaces.Notification;
using Domain.Interfaces.Repositories;
using Domain.Services;
using Domain.Utilities;
using Microsoft.Extensions.Logging;
using Moq;
using UnitTests.Fixtures;
using UnitTests.Mocks;
using UnitTests.Mocks.Repositories;

namespace UnitTests.Services
{
    public class MotorcycleServiceTests
    {
        private readonly Mock<IMotoRepository> _motorcycleRepositoryMock;
        private readonly Mock<INotificationContext> _notificationContextMock;
        private readonly Mock<ILogger<MotorcycleService>> _loggerMock;

        public MotorcycleServiceTests()
        {
            _motorcycleRepositoryMock = MotorcycleRepositoryMock.Create();
            _notificationContextMock = NotificationContextMock.Create();
            _loggerMock = LoggerMock.Create<MotorcycleService>();
        }

        [Fact]
        public void GetByLicensePlate_ValidLicensePlate_ReturnMotorcyles()
        {
            // Arrange
            var motorcycles = MotorcycleFixture.CreateList(3);
            var licensePlate = motorcycles.First().LicensePlate;

            _motorcycleRepositoryMock.SetupGetByLicensePlate(motorcycles);

            var service = new MotorcycleService(_motorcycleRepositoryMock.Object, _notificationContextMock.Object, _loggerMock.Object);

            // Act
            var result = service.GetByLicensePlate(licensePlate);

            // Assert
            Assert.Equal(licensePlate, result?.FirstOrDefault().LicensePlate);
        }

        [Fact]
        public async Task InsertMotorcycle_UniqueLicensePlate_Insert()
        {
            // Arrange
            var motorcycle = MotorcycleFixture.Create();

            _motorcycleRepositoryMock.SetupGetByLicensePlate(new List<Motorcycle>());
            _motorcycleRepositoryMock.SetupInsertAsync(motorcycle);

            var service = new MotorcycleService(_motorcycleRepositoryMock.Object, _notificationContextMock.Object, _loggerMock.Object);

            // Act
            await service.InsertMotorcycleAsync(motorcycle);

            // Assert
            _notificationContextMock.Verify(nc => nc.AddNotification(It.IsAny<string>()), Times.Never);
            _motorcycleRepositoryMock.Verify(mr => mr.InsertAsync(motorcycle), Times.Once);
        }

        [Fact]
        public async Task InsertMotorcycle_UnavailableLicensePlate_NotifyAndDontInsert()
        {
            // Arrange
            var motorcycles = MotorcycleFixture.CreateList(2);
            var motorcycle = MotorcycleFixture.Create();

            motorcycle.LicensePlate = motorcycles.First().LicensePlate;

            _motorcycleRepositoryMock.SetupGetByLicensePlate(motorcycles);

            var service = new MotorcycleService(_motorcycleRepositoryMock.Object, _notificationContextMock.Object, _loggerMock.Object);

            // Act
            await service.InsertMotorcycleAsync(motorcycle);

            // Assert
            _notificationContextMock.Verify(nc => nc.AddNotification(ErrorNotifications.LICENSE_PLATE_USED), Times.Once);
            _motorcycleRepositoryMock.Verify(mr => mr.InsertAsync(motorcycle), Times.Never);
        }

        [Fact]
        public void UpdateMotorcycleLicensePlate_IdAndLicensePlateValid_Update()
        {
            // Arrange
            var motorcycle = MotorcycleFixture.Create();
            var newLicensePlate = "ABC1234";

            _motorcycleRepositoryMock.SetupGet(new List<Motorcycle> { motorcycle });
            _motorcycleRepositoryMock.SetupGetByLicensePlate(new List<Motorcycle>());

            var service = new MotorcycleService(_motorcycleRepositoryMock.Object, _notificationContextMock.Object, _loggerMock.Object);

            // Act
            var result = service.UpdateMotorcycleLicensePlate(motorcycle.Id, newLicensePlate);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newLicensePlate, result.LicensePlate);
            _notificationContextMock.Verify(nc => nc.AddNotification(It.IsAny<string>()), Times.Never);
            _motorcycleRepositoryMock.Verify(mr => mr.Update(motorcycle), Times.Once);
        }

        [Fact]
        public void UpdateMotorcycleLicensePlate_InvalidId_NotifyAndDontUpdate()
        {
            // Arrange
            var motorcycles = MotorcycleFixture.CreateList(2);
            var motorcycle = MotorcycleFixture.Create();

            _motorcycleRepositoryMock.SetupGet(motorcycles);
            _motorcycleRepositoryMock.SetupGetByLicensePlate(motorcycles);

            var service = new MotorcycleService(_motorcycleRepositoryMock.Object, _notificationContextMock.Object, _loggerMock.Object);

            // Act
            var result = service.UpdateMotorcycleLicensePlate(motorcycle.Id, motorcycle.LicensePlate);

            // Assert
            Assert.Null(result);
            _notificationContextMock.Verify(nc => nc.AddNotification(ErrorNotifications.MOTORCYCLE_NOT_FOUND), Times.Once);
            _motorcycleRepositoryMock.Verify(mr => mr.Update(motorcycle), Times.Never);
        }

        [Fact]
        public void UpdateMotorcycleLicensePlate_UnavailableLicensePlate_NotifyAndDontUpdate()
        {
            // Arrange
            var motorcycles = MotorcycleFixture.CreateList(2);
            var motorcycle = MotorcycleFixture.Create();

            motorcycle.LicensePlate = motorcycles.First().LicensePlate;

            motorcycles.Add(motorcycle);

            _motorcycleRepositoryMock.SetupGet(motorcycles);
            _motorcycleRepositoryMock.SetupGetByLicensePlate(motorcycles);

            var service = new MotorcycleService(_motorcycleRepositoryMock.Object, _notificationContextMock.Object, _loggerMock.Object);

            // Act
            var result = service.UpdateMotorcycleLicensePlate(motorcycle.Id, motorcycle.LicensePlate);

            // Assert
            Assert.Null(result);
            _notificationContextMock.Verify(nc => nc.AddNotification(ErrorNotifications.LICENSE_PLATE_USED), Times.Once);
            _motorcycleRepositoryMock.Verify(mr => mr.Update(motorcycle), Times.Never);
        }

        [Fact]
        public void UpdateMotorcycleLicensePlate_ValidMotorcycle_Update()
        {
            // Arrange
            var motorcycles = MotorcycleFixture.CreateList(1);
            var motorcycle = motorcycles.First();

            _motorcycleRepositoryMock.SetupGet(motorcycles);

            var service = new MotorcycleService(_motorcycleRepositoryMock.Object, _notificationContextMock.Object, _loggerMock.Object);

            // Act
            service.UpdateMotorcycle(motorcycle);

            // Assert
            _notificationContextMock.Verify(nc => nc.AddNotification(It.IsAny<string>()), Times.Never);
            _motorcycleRepositoryMock.Verify(mr => mr.Update(motorcycle), Times.Once);
        }

        [Fact]
        public void DeleteMotorcycle_ValidMotorcycle_Delete()
        {
            // Arrange
            var motorcycles = MotorcycleFixture.CreateList(1);
            var motorcycle = motorcycles.First();

            _motorcycleRepositoryMock.SetupGetLocacoes(motorcycles);

            var service = new MotorcycleService(_motorcycleRepositoryMock.Object, _notificationContextMock.Object, _loggerMock.Object);

            // Act
            service.DeleteMotorcycle(motorcycle.Id);

            // Assert
            _notificationContextMock.Verify(nc => nc.AddNotification(It.IsAny<string>()), Times.Never);
            _motorcycleRepositoryMock.Verify(mr => mr.Delete(motorcycle.Id), Times.Once);
        }

        [Fact]
        public void DeleteMotorcycle_MotorcycleHaveRentals_NotifyAndDontDelete()
        {
            // Arrange
            var motorcycle = MotorcycleFixture.Create();
            motorcycle.Rentals.Add(RentalFixture.Create());
            var motorcycles = MotorcycleFixture.CreateList(1);
            motorcycles.Add(motorcycle);

            _motorcycleRepositoryMock.SetupGetLocacoes(motorcycles);

            var service = new MotorcycleService(_motorcycleRepositoryMock.Object, _notificationContextMock.Object, _loggerMock.Object);

            // Act
            service.DeleteMotorcycle(motorcycle.Id);

            // Assert
            _notificationContextMock.Verify(nc => nc.AddNotification(ErrorNotifications.MOTORCYCLE_WITH_RENTALS), Times.Once);
            _motorcycleRepositoryMock.Verify(mr => mr.Delete(It.IsAny<Guid>()), Times.Never);
        }

        [Fact]
        public void DeleteMotorcycle_InvalidId_NotifyAndDontDelete()
        {
            // Arrange
            var motorcycleId = Guid.NewGuid();

            _motorcycleRepositoryMock.SetupGet(new List<Motorcycle>());

            var service = new MotorcycleService(_motorcycleRepositoryMock.Object, _notificationContextMock.Object, _loggerMock.Object);

            // Act
            service.DeleteMotorcycle(motorcycleId);

            // Assert
            _notificationContextMock.Verify(nc => nc.AddNotification(ErrorNotifications.MOTORCYCLE_NOT_FOUND), Times.Once);
            _motorcycleRepositoryMock.Verify(mr => mr.Delete(It.IsAny<Guid>()), Times.Never);
        }

    }
}
