using Domain.Entities;
using Domain.Interfaces.Notification;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Services;
using Domain.Utilities;
using Microsoft.Extensions.Logging;
using Moq;
using UnitTests.Fixtures;
using UnitTests.Mocks;
using UnitTests.Mocks.Repositories;
using UnitTests.Mocks.Services;

namespace UnitTests.Services
{
    public class RentalServiceTests
    {
        private Mock<IRentalRepository> _rentalRepositoryMock;
        private Mock<ICourierService> _courierServiceMock;
        private Mock<IMotorcycleService> _motocycleServiceMock;
        private Mock<INotificationContext> _notificationContextMock;
        private Mock<ILogger<RentalService>> _loggerMock;

        public RentalServiceTests()
        {
            _rentalRepositoryMock = RentalRepositoryMock.Create();
            _courierServiceMock = CourierServiceMock.Create();
            _motocycleServiceMock = MotorcycleServiceMock.Create();
            _notificationContextMock = NotificationContextMock.Create();
            _loggerMock = LoggerMock.Create<RentalService>();
        }

        [Fact]
        public async Task InsertRentalAsync_AvailableMotorcycleAndValidCourier_Insert()
        {
            // Arrange
            var couriers = CourierFixture.CreateList(1);
            var courier = CourierFixture.Create();
            couriers.Add(courier);

            var rental = RentalFixture.Create();
            rental.CourierId = courier.Id;

            var motos = MotorcycleFixture.CreateList(1);

            _motocycleServiceMock.SetupGetMotosDisponiveis(motos);

            var logLevel = LogLevel.Information;
            _loggerMock.SetupLogLevel(logLevel);

            _rentalRepositoryMock.SetupInsertAsync(rental);
            _courierServiceMock.SetupGetRentals(couriers);

            var service = new RentalService(_rentalRepositoryMock.Object,
                                                    _courierServiceMock.Object,
                                                    _motocycleServiceMock.Object,
                                                    _notificationContextMock.Object,
                                                    _loggerMock.Object);

            // Act
            await service.InsertRentalAsync(rental);

            // Assert
            _motocycleServiceMock.Verify(ms => ms.GetAvailable(), Times.Once());
            _notificationContextMock.Verify(nc => nc.AddNotification(It.IsAny<string>()), Times.Never);
            _rentalRepositoryMock.Verify(lr => lr.InsertAsync(rental), Times.Once());

        }

        [Fact]
        public async Task InsertRentalAsync_NoMotorcyclesAvailable_NotifyAndDontInsert()
        {
            // Arrange
            var rental = RentalFixture.Create();
            _motocycleServiceMock.SetupGetMotosDisponiveis(new List<Motorcycle>());

            var logLevel = LogLevel.Information;
            _loggerMock.SetupLogLevel(logLevel);

            var service = new RentalService(_rentalRepositoryMock.Object,
                                                    _courierServiceMock.Object,
                                                    _motocycleServiceMock.Object,
                                                    _notificationContextMock.Object,
                                                    _loggerMock.Object);

            // Act
            await service.InsertRentalAsync(rental);

            // Assert
            _motocycleServiceMock.Verify(ms => ms.GetAvailable(), Times.Once());
            _notificationContextMock.Verify(nc => nc.AddNotification(ErrorNotifications.NO_MOTORCYCLES_AVAILABLE), Times.Once);
        }

        [Fact]
        public async Task InsertRentalAsync_CourierNotFound_NotifyAndDontInsert()
        {
            // Arrange
            var rental = RentalFixture.Create();
            var courierId = rental.CourierId;

            _motocycleServiceMock.SetupGetMotosDisponiveis(new List<Motorcycle> { MotorcycleFixture.Create() });
            _courierServiceMock.SetupGetRentals(new List<Courier>());

            var service = new RentalService(_rentalRepositoryMock.Object,
                                                    _courierServiceMock.Object,
                                                    _motocycleServiceMock.Object,
                                                    _notificationContextMock.Object,
                                                    _loggerMock.Object);

            // Act
            await service.InsertRentalAsync(rental);

            // Assert
            _notificationContextMock.Verify(nc => nc.AddNotification(ErrorNotifications.COURIER_NOT_FOUND), Times.Once);
            _courierServiceMock.Verify(r => r.GetRentals(courierId), Times.Once);
            _rentalRepositoryMock.Verify(r => r.InsertAsync(rental), Times.Never);
        }

        [Fact]
        public async Task InsertRentalAsync_CourierWithActiveRental_NotifyAndDontInsert()
        {
            // Arrange
            var activeRental = RentalFixture.Create();
            var rental = RentalFixture.Create();
            var courier = CourierFixture.Create();

            courier.Rentals.Add(activeRental);
            rental.CourierId = courier.Id;

            _motocycleServiceMock.SetupGetMotosDisponiveis(new List<Motorcycle> { MotorcycleFixture.Create() });
            _courierServiceMock.SetupGetRentals(new List<Courier> { courier });

            var service = new RentalService(_rentalRepositoryMock.Object,
                                                    _courierServiceMock.Object,
                                                    _motocycleServiceMock.Object,
                                                    _notificationContextMock.Object,
                                                    _loggerMock.Object);

            // Act
            await service.InsertRentalAsync(rental);

            // Assert
            _notificationContextMock.Verify(nc => nc.AddNotification(ErrorNotifications.COURIER_RENTAL_ACTIVE), Times.Once);
            _rentalRepositoryMock.Verify(r => r.InsertAsync(rental), Times.Never);
        }

        [Fact]
        public async Task InsertRentalAsync_CourierWithoutCnhTypeA_NotifyAndDontInsert()
        {
            // Arrange
            var rental = RentalFixture.Create();
            var courier = CourierFixture.Create();

            courier.CnhType = CnhType.B;
            rental.CourierId = courier.Id;

            _motocycleServiceMock.SetupGetMotosDisponiveis(new List<Motorcycle> { MotorcycleFixture.Create() });
            _courierServiceMock.SetupGetRentals(new List<Courier> { courier });

            var service = new RentalService(_rentalRepositoryMock.Object,
                                                    _courierServiceMock.Object,
                                                    _motocycleServiceMock.Object,
                                                    _notificationContextMock.Object,
                                                    _loggerMock.Object);

            // Act
            await service.InsertRentalAsync(rental);

            // Assert
            _notificationContextMock.Verify(nc => nc.AddNotification(ErrorNotifications.COURIER_WITHOUT_CNHTYPE_A), Times.Once);
            _rentalRepositoryMock.Verify(r => r.InsertAsync(rental), Times.Never);
        }

        [Fact]
        public void ReportReturn_RentalNotFound_NotifyAndDontReturnsRentalPrice()
        {
            // Arrange
            var rental = RentalFixture.Create();
            var courier = CourierFixture.Create();

            _rentalRepositoryMock.SetupGet(new List<Rental>());

            var service = new RentalService(_rentalRepositoryMock.Object,
                                                    _courierServiceMock.Object,
                                                    _motocycleServiceMock.Object,
                                                    _notificationContextMock.Object,
                                                    _loggerMock.Object);

            // Act
            var result = service.ReportReturn(rental.Id, DateTime.Now, courier.Id);

            // Assert
            _notificationContextMock.Verify(nc => nc.AddNotification(ErrorNotifications.RENTAL_NOT_FOUND), Times.Once);
            Assert.Equal(0, result);
        }

        [Fact]
        public void ReportReturn_CourierNotAuthorized_NotifyAndDontReturnsRentalPrice()
        {
            // Arrange
            var rental = RentalFixture.Create();
            var courier = CourierFixture.Create();

            _rentalRepositoryMock.SetupGet(new List<Rental> { rental });

            var service = new RentalService(_rentalRepositoryMock.Object,
                                                    _courierServiceMock.Object,
                                                    _motocycleServiceMock.Object,
                                                    _notificationContextMock.Object,
                                                    _loggerMock.Object);

            // Act
            var result = service.ReportReturn(rental.Id, DateTime.Now, courier.Id);

            // Assert
            _notificationContextMock.Verify(nc => nc.AddNotification(ErrorNotifications.RENTAL_COURIER_NOT_AUTHORIZED), Times.Once);
            Assert.Equal(0, result);
        }

        [Fact]
        public void ReportReturn_InactiveRental_NotifyAndDontReturnsRentalPrice()
        {
            // Arrange
            var rental = RentalFixture.Create();
            rental.Active = false;
            var courier = CourierFixture.Create();
            rental.CourierId = courier.Id;

            _rentalRepositoryMock.SetupGet(new List<Rental> { rental });

            var service = new RentalService(_rentalRepositoryMock.Object,
                                                    _courierServiceMock.Object,
                                                    _motocycleServiceMock.Object,
                                                    _notificationContextMock.Object,
                                                    _loggerMock.Object);

            // Act
            var result = service.ReportReturn(rental.Id, DateTime.Now, courier.Id);

            // Assert
            _notificationContextMock.Verify(nc => nc.AddNotification(ErrorNotifications.RENTAL_INACTIVE), Times.Once);
            Assert.Equal(0, result);
        }

        [Fact]
        public void ReportReturn_ValidRental_Sucess()
        {
            // Arrange
            var previsaoDevolucao = DateTime.Now.AddDays(7);
            var rental = RentalFixture.Create(Plan.A);
            var courier = CourierFixture.Create();
            var motorcycle = MotorcycleFixture.Create();
            motorcycle.Available = false;
            rental.CourierId = courier.Id;
            rental.Motorcycle = motorcycle;

            _rentalRepositoryMock.SetupGet(new List<Rental> { rental });

            var service = new RentalService(_rentalRepositoryMock.Object,
                                                    _courierServiceMock.Object,
                                                    _motocycleServiceMock.Object,
                                                    _notificationContextMock.Object,
                                                    _loggerMock.Object);

            // Act
            var preco = service.ReportReturn(rental.Id, previsaoDevolucao, courier.Id);

            // Assert
            Assert.Equal(210, preco);
            Assert.True(motorcycle.Available);
        }

    }

}
