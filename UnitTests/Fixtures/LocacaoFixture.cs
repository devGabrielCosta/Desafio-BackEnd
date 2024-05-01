using Bogus;
using Dominio.Entities;

namespace UnitTests.Fixtures
{

    public static class LocacaoFixture
    {
        private static readonly Faker<Locacao> _faker;

        static LocacaoFixture()
        {
            _faker = new Faker<Locacao>()
                .CustomInstantiator(f => new Locacao(
                    f.Random.Enum<Plano>(),
                    f.Random.Guid())
                );
        }

        public static Locacao Create()
        {
            return _faker.Generate();
        }

        public static Locacao Create(Plano plano)
        {
            return _faker.CustomInstantiator(_ => new Locacao(plano, Guid.NewGuid()));
        }

        public static List<Locacao> CreateList(int qtd)
        {
            return _faker.Generate(qtd);
        }
    }
}
