using Bogus;
using Dominio.Entities;

namespace UnitTests.Fixtures
{
    public static class PedidoFixture
    {
        private static readonly Faker<Pedido> _faker;

        static PedidoFixture()
        {
            _faker = new Faker<Pedido>()
                .CustomInstantiator(f => new Pedido(
                    f.Finance.Amount()
                ));
        }

        public static Pedido Create(Situacao situacao = Situacao.Disponivel)
        {
            var pedido = _faker.Generate();
            pedido.Situacao = situacao;
            return pedido;
        }

        public static List<Pedido> CreateList(int qtd)
        {
            return _faker.Generate(qtd);
        }
    }
}
