using Moq;
using Domain.Entities;
using Domain.Handlers.Commands;
using Domain.Handlers;
using UnitTests.Fixtures;
using Domain.Interfaces.Repositories;
using UnitTests.Mocks.Repositories;

namespace UnitTests.Handlers
{
    public class NotifyOrderCouriersHandlerTests
    {
        private Mock<ICourierRepository> _courierRepositoryMock;
        private Mock<IOrderRepository> _orderRepositoryMock;

        public NotifyOrderCouriersHandlerTests() 
        {
            _courierRepositoryMock = CourierRepositoryMock.Create();
            _orderRepositoryMock = OrderRepositoryMock.Create();
        }

        [Fact]
        public void Handle_NoAvailablesCouriers_ThrowsException()
        {
            // Arrange
            var orders = OrderFixture.CreateList(1);

            _courierRepositoryMock.SetupAvailableCouriersForOrder(new List<Courier>());
            _orderRepositoryMock.SetupGet(orders);

            var handler = new NotifyOrderCouriersHandler(_courierRepositoryMock.Object, _orderRepositoryMock.Object);

            // Act
            Action act = () => handler.Handle(new NotifyOrderCouriersCommand(Guid.NewGuid()));

            // Assert
            Assert.Throws<Exception>(act);
            _orderRepositoryMock.Verify(repo => repo.Update(It.IsAny<Order>()), Times.Never);
        }

        [Fact]
        public void Handle_OrderNotFound_ThrowsArgumentException()
        {
            // Arrange
            var couriers = CourierFixture.CreateList(1);

            _courierRepositoryMock.SetupAvailableCouriersForOrder(couriers);
            _orderRepositoryMock.SetupGet(new List<Order>());

            var handler = new NotifyOrderCouriersHandler(_courierRepositoryMock.Object, _orderRepositoryMock.Object);

            // Act
            Action act = () => handler.Handle(new NotifyOrderCouriersCommand(Guid.NewGuid()));

            // Assert
            var exception = Assert.Throws<ArgumentException>(act);
            _orderRepositoryMock.Verify(repo => repo.Update(It.IsAny<Order>()), Times.Never);
        }

        [Fact]
        public void Handle_CouriersAndOrderFound_UpdatesOrder()
        {
            // Arrange
            var couriers = CourierFixture.CreateList(2);
            var orders = OrderFixture.CreateList(1);

            var order = OrderFixture.Create(Status.Available);
            orders.Add(order);
            
            _courierRepositoryMock.SetupAvailableCouriersForOrder(couriers);
            _orderRepositoryMock.SetupGet(orders);

            var handler = new NotifyOrderCouriersHandler(_courierRepositoryMock.Object, _orderRepositoryMock.Object);

            // Act
            handler.Handle(new NotifyOrderCouriersCommand(order.Id));

            // Assert
            foreach (var courier in couriers)
            {
                Assert.Contains(courier, order.Notifieds);
            }
            Assert.Equal(couriers.Count, order.Notifieds.Count);
            _orderRepositoryMock.Verify(repo => repo.Update(order), Times.Once);
        }
    }
}

