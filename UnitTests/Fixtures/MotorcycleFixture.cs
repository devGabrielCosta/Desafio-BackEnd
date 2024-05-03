using Bogus;
using Domain.Entities;

namespace UnitTests.Fixtures
{
    public static class MotorcycleFixture
    {
        private static readonly Faker<Motorcycle> _faker;

        static MotorcycleFixture()
        {
            _faker = new Faker<Motorcycle>()
                .CustomInstantiator(f => new Motorcycle(
                    f.Random.Number(1950, 3000),
                    f.Random.AlphaNumeric(25),
                    f.Random.AlphaNumeric(7)

                ));
        }

        public static Motorcycle Create()
        {
            return _faker.Generate();
        }

        public static List<Motorcycle> CreateList(int count)
        {
            return _faker.Generate(count);
        }
    }
}
