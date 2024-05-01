using Bogus;
using Dominio.Entities;

namespace UnitTests.Fixtures
{
    public static class MotoFixture
    {
        private static readonly Faker<Moto> _faker;

        static MotoFixture()
        {
            _faker = new Faker<Moto>()
                .CustomInstantiator(f => new Moto(
                    f.Random.Number(1950, 3000),
                    f.Random.AlphaNumeric(25),
                    f.Random.AlphaNumeric(7)

                ));
        }

        public static Moto Create(string cnhTipo = "a")
        {
            return _faker.Generate();
        }

        public static List<Moto> CreateList(int qtd)
        {
            return _faker.Generate(qtd);
        }
    }
}
