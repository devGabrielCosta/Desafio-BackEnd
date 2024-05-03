using Domain.Entities;
using Domain.Interfaces.Repositories;
using Moq;

namespace UnitTests.Mocks.Repositories
{
    public static class OrderRepositoryMock
    {
        public static Mock<IOrderRepository> Create()
        {
            return new Mock<IOrderRepository>();
        }

        public static void SetupGet(this Mock<IOrderRepository> mock, List<Order> orders)
        {
            mock.Setup(repo => repo.Get()).Returns(orders.AsQueryable());
            mock.Setup(repo => repo.Get(It.IsAny<Guid>())).Returns((Guid id) => orders.Where(p => p.Id == id).AsQueryable());
        }

        public static void SetupGetNotifieds(this Mock<IOrderRepository> mock, List<Order> orders)
        {
            mock.Setup(repo => repo.GetNotifieds(It.IsAny<Guid>())).Returns((Guid id) => orders.Where(p => p.Id == id).AsQueryable());
        }

        public static void SetupInsertAsync(this Mock<IOrderRepository> mock, Order order)
        {
            mock.Setup(repo => repo.InsertAsync(It.IsAny<Order>())).Returns(Task.FromResult(order));
        }
    }
}
