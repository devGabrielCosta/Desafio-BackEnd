using Bogus;
using Domain.Entities;

namespace UnitTests.Fixtures
{

    public static class RentalFixture
    {
        private static readonly Faker<Rental> _faker;

        static RentalFixture()
        {
            _faker = new Faker<Rental>()
                .CustomInstantiator(f => new Rental(
                    f.Random.Enum<Plan>(),
                    f.Random.Guid())
                );
        }

        public static Rental Create()
        {
            return _faker.Generate();
        }

        public static Rental Create(Plan plan)
        {
            return _faker.CustomInstantiator(_ => new Rental(plan, Guid.NewGuid()));
        }

        public static List<Rental> CreateList(int count)
        {
            return _faker.Generate(count);
        }
    }
}
