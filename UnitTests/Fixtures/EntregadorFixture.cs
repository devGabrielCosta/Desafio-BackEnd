using Bogus;
using Bogus.Extensions.Brazil;
using Dominio.Entities;

namespace UnitTests.Fixtures
{

    public static class EntregadorFixture
    {
        private static readonly Faker<Entregador> _faker;

        static EntregadorFixture()
        {
            _faker = new Faker<Entregador>()
                .CustomInstantiator(f => new Entregador(
                    f.Person.FullName,
                    f.Company.Cnpj().Replace(".", "").Replace("-", "").Replace("/", ""),
                    f.Date.Past(30),
                    f.Random.String2(12, "0123456789"),
                    f.Random.String2(1, "ABCDE")
                ))
                .RuleFor(e => e.CnhImagem, f => f.Image.PicsumUrl());
        }

        public static Entregador Create(string cnhTipo = "a")
        {
            var entregador = _faker.Generate();
            entregador.CnhTipo = cnhTipo;
            return entregador;
        }

        public static List<Entregador> CreateList(int qtd)
        {
            return _faker.Generate(qtd);
        }
    }
}
