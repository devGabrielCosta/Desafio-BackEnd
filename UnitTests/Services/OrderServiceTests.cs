using Domain.Entities;
using Domain.Handlers.Commands;
using Domain.Interfaces.Messaging;
using Domain.Interfaces.Notification;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Services;
using Microsoft.Extensions.Logging;
using Moq;
using UnitTests.Fixtures;
using UnitTests.Mocks.Repositories;
using UnitTests.Mocks.Services;
using UnitTests.Mocks;
using Domain.Utilities;

namespace UnitTests.Services
{
    public class OrderServiceTests
    {
        private readonly Mock<IOrderRepository> _orderRepositoryMock;
        private readonly Mock<IPublisher<NotifyOrderCouriersCommand>> _publisherNotificacaoMock;
        private readonly Mock<ICourierService> _courierServiceMock;
        private readonly Mock<INotificationContext> _notificationContextMock;
        private readonly Mock<ILogger<OrderService>> _loggerMock;

        public OrderServiceTests()
        {
            _orderRepositoryMock = OrderRepositoryMock.Create();
            _publisherNotificacaoMock = new Mock<IPublisher<NotifyOrderCouriersCommand>>();
            _publisherNotificacaoMock.Setup(x => x.Publish(It.IsAny<NotifyOrderCouriersCommand>()));
            _courierServiceMock = CourierServiceMock.Create();
            _notificationContextMock = NotificationContextMock.Create();
            _loggerMock = LoggerMock.Create<OrderService>();
        }

        [Fact]
        public void GetNotifieds_OrderExists_Return()
        {
            // Arrange
            var orders = OrderFixture.CreateList(2);
            var order = orders.First();

            _orderRepositoryMock.SetupGetNotifieds(orders);

            var service = new OrderService(
                _orderRepositoryMock.Object,
                _publisherNotificacaoMock.Object,
                _courierServiceMock.Object,
                _notificationContextMock.Object,
                _loggerMock.Object);

            // Act
            var result = service.GetNotifieds(order.Id);

            // Assert
            Assert.Equal(order, result);
        }

        [Fact]
        public void Get_ReturnOrders()
        {
            // Arrange
            var orders = OrderFixture.CreateList(2);

            _orderRepositoryMock.SetupGet(orders);

            var service = new OrderService(
                _orderRepositoryMock.Object,
                _publisherNotificacaoMock.Object,
                _courierServiceMock.Object,
                _notificationContextMock.Object,
                _loggerMock.Object);

            // Act
            var result = service.Get();

            // Assert
            Assert.Equal(orders, result);
        }

        [Fact]
        public async Task InsertOrderAsync_Insert()
        {
            // Arrange
            var order = OrderFixture.Create();

            _orderRepositoryMock.SetupInsertAsync(order);

            var service = new OrderService(
                _orderRepositoryMock.Object,
                _publisherNotificacaoMock.Object,
                _courierServiceMock.Object,
                _notificationContextMock.Object,
                _loggerMock.Object);

            // Act
            var result = await service.InsertOrderAsync(order);

            // Assert
            _publisherNotificacaoMock.Verify(publisher => publisher.Publish(It.IsAny<NotifyOrderCouriersCommand>()), Times.Once);
            Assert.Equal(order, result);
        }

        [Fact]
        public void AcceptOrder_OrderAndCourierValid_Success()
        {
            // Arrange
            var couriers = CourierFixture.CreateList(1);
            var courier = couriers.First();

            var order = OrderFixture.Create(Status.Available);
            order.Notifieds.Add(courier);
            var orders = OrderFixture.CreateList(1);
            orders.Add(order);
            
            _orderRepositoryMock.SetupGetNotifieds(orders);
            _courierServiceMock.SetupGet(couriers);

            var service = new OrderService(
                _orderRepositoryMock.Object,
                _publisherNotificacaoMock.Object,
                _courierServiceMock.Object,
                _notificationContextMock.Object,
                _loggerMock.Object);

            // Act
            service.AcceptOrder(order.Id, courier.Id);

            // Assert
            _orderRepositoryMock.Verify(repo => repo.Update(order), Times.Once);
            Assert.Equal(Status.Accepted, order.Status);
            Assert.Equal(courier, order.Courier);
        }

        [Fact]
        public void AcceptOrder_CourierNotFound_NotifyAndDontUpdate()
        {
            // Arrange
            var order = OrderFixture.Create(Status.Available);
            var orders = OrderFixture.CreateList(1);
            orders.Add(order);

            var couriers = CourierFixture.CreateList(1);
            var courier = CourierFixture.Create();

            _orderRepositoryMock.SetupGetNotifieds(orders);
            _courierServiceMock.SetupGet(couriers);

            var service = new OrderService(
                _orderRepositoryMock.Object,
                _publisherNotificacaoMock.Object,
                _courierServiceMock.Object,
                _notificationContextMock.Object,
                _loggerMock.Object);

            // Act
            service.AcceptOrder(order.Id, courier.Id);

            // Assert
            _orderRepositoryMock.Verify(repo => repo.Update(order), Times.Never);
            _notificationContextMock.Verify(nc => nc.AddNotification(ErrorNotifications.COURIER_NOT_FOUND), Times.Once);
        }

        [Fact]
        public void AcceptOrder_OrderNotFound_NotifyAndDontUpdate()
        {
            // Arrange
            var order = OrderFixture.Create();
            var orders = OrderFixture.CreateList(1);

            var couriers = CourierFixture.CreateList(1);
            var courier = couriers.First();

            _orderRepositoryMock.SetupGetNotifieds(orders);
            _courierServiceMock.SetupGet(couriers);

            var service = new OrderService(
                _orderRepositoryMock.Object,
                _publisherNotificacaoMock.Object,
                _courierServiceMock.Object,
                _notificationContextMock.Object,
                _loggerMock.Object);

            // Act
            service.AcceptOrder(order.Id, courier.Id);

            // Assert
            _orderRepositoryMock.Verify(repo => repo.Update(It.IsAny<Order>()), Times.Never);
            _notificationContextMock.Verify(nc => nc.AddNotification(ErrorNotifications.ORDER_NOT_FOUND), Times.Once);
        }

        [Fact]
        public void AcceptOrder_CourierNotNotified_NotifyAndDontUpdate()
        {
            // Arrange
            var order = OrderFixture.Create();
            var orders = OrderFixture.CreateList(1);
            orders.Add(order);

            var couriers = CourierFixture.CreateList(1);
            var courier = CourierFixture.Create();
            couriers.Add(courier);

            _orderRepositoryMock.SetupGetNotifieds(orders);
            _courierServiceMock.SetupGet(couriers);

            var service = new OrderService(
                _orderRepositoryMock.Object,
                _publisherNotificacaoMock.Object,
                _courierServiceMock.Object,
                _notificationContextMock.Object,
                _loggerMock.Object);

            // Act
            service.AcceptOrder(order.Id, courier.Id);

            // Assert
            _orderRepositoryMock.Verify(repo => repo.Update(It.IsAny<Order>()), Times.Never);
            _notificationContextMock.Verify(nc => nc.AddNotification(ErrorNotifications.COURIER_NOT_NOTIFIED), Times.Once);
        }

        [Fact]
        public void AcceptOrder_OrderNotAvailable_NotifyAndDontUpdate()
        {
            // Arrange
            var couriers = CourierFixture.CreateList(1);
            var courier = CourierFixture.Create();
            couriers.Add(courier);

            var order = OrderFixture.Create(Status.Accepted);
            order.Notifieds.Add(courier);
            var orders = OrderFixture.CreateList(1);
            orders.Add(order);

            _orderRepositoryMock.SetupGetNotifieds(orders);
            _courierServiceMock.SetupGet(couriers);

            var service = new OrderService(
                _orderRepositoryMock.Object,
                _publisherNotificacaoMock.Object,
                _courierServiceMock.Object,
                _notificationContextMock.Object,
                _loggerMock.Object);

            // Act
            service.AcceptOrder(order.Id, courier.Id);

            // Assert
            _orderRepositoryMock.Verify(repo => repo.Update(It.IsAny<Order>()), Times.Never);
            _notificationContextMock.Verify(nc => nc.AddNotification(ErrorNotifications.ORDER_UNAVAILABLE), Times.Once);
        }

        [Fact]
        public void FinishOrder_OrderAndCourierValid_Sucess()
        {
            // Arrange
            var courier = CourierFixture.Create();

            var order = OrderFixture.Create(Status.Accepted);
            order.CourierId = courier.Id;
            var orders = OrderFixture.CreateList(1);
            orders.Add(order);

            _orderRepositoryMock.SetupGet(orders);

            var logLevel = LogLevel.Information;
            _loggerMock.SetupLogLevel(logLevel);

            var service = new OrderService(
                _orderRepositoryMock.Object,
                _publisherNotificacaoMock.Object,
                _courierServiceMock.Object,
                _notificationContextMock.Object,
                _loggerMock.Object);

            // Act
            service.FinishOrder(order.Id, courier.Id);

            // Assert
            _orderRepositoryMock.Verify(repo => repo.Update(order), Times.Once);
            Assert.Equal(Status.Delivered, order.Status);
        }

        [Fact]
        public void FinishOrder_CourierNotAuthorized_NotifyAndDontUpdate()
        {
            // Arrange
            var courier = CourierFixture.Create();

            var order = OrderFixture.Create(Status.Accepted);
            var orders = OrderFixture.CreateList(1);
            orders.Add(order);

            _orderRepositoryMock.SetupGet(orders);

            var service = new OrderService(
                _orderRepositoryMock.Object,
                _publisherNotificacaoMock.Object,
                _courierServiceMock.Object,
                _notificationContextMock.Object,
                _loggerMock.Object);

            // Act
            service.FinishOrder(order.Id, courier.Id);

            // Assert
            _orderRepositoryMock.Verify(repo => repo.Update(It.IsAny<Order>()), Times.Never);
            _notificationContextMock.Verify(nc => nc.AddNotification(ErrorNotifications.ORDER_COURIER_NOT_AUTHORIZED), Times.Once);
        }

        [Fact]
        public void FinishOrder_OrderNotAcceptedOrDelivered_NotifyAndDontUpdate()
        {
            // Arrange
            var courier = CourierFixture.Create();

            var order = OrderFixture.Create(Status.Available);
            order.CourierId = courier.Id;
            var orders = OrderFixture.CreateList(1);
            orders.Add(order);

            _orderRepositoryMock.SetupGet(orders);

            var service = new OrderService(
                _orderRepositoryMock.Object,
                _publisherNotificacaoMock.Object,
                _courierServiceMock.Object,
                _notificationContextMock.Object,
                _loggerMock.Object);

            // Act
            service.FinishOrder(order.Id, courier.Id);

            // Assert
            _orderRepositoryMock.Verify(repo => repo.Update(It.IsAny<Order>()), Times.Never);
            _notificationContextMock.Verify(nc => nc.AddNotification(ErrorNotifications.ORDER_NOT_ACCEPTED_OR_DELIVERED), Times.Once);
        }
    }
}
