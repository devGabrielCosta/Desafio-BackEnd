using Bogus;
using Domain.Entities;

namespace UnitTests.Fixtures
{
    public static class OrderFixture
    {
        private static readonly Faker<Order> _faker;

        static OrderFixture()
        {
            _faker = new Faker<Order>()
                .CustomInstantiator(f => new Order(
                    f.Finance.Amount()
                ));
        }

        public static Order Create(Status status = Status.Available)
        {
            var order = _faker.Generate();
            order.Status = status;
            return order;
        }

        public static List<Order> CreateList(int count)
        {
            return _faker.Generate(count);
        }
    }
}
