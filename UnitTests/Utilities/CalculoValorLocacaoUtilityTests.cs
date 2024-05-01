using Dominio.Entities;
using Dominio.Utilities;
using UnitTests.Fixtures;

namespace UnitTests.Utilities
{
    public class CalculoValorLocacaoUtilityTests
    {
        [Fact]
        public void CalcularValor_PrazoNormal_DeveCalcularCorretamente()
        {
            // Arrange
            var locacao = LocacaoFixture.Create(Plano.A);
            locacao.PrevisaoDevolucao = DateTime.Now.AddDays(7);

            // Act
            var valorCalculado = CalculoValorLocacaoUtility.CalcularValor(locacao);

            // Assert
            Assert.Equal(210, valorCalculado);
        }

        [Fact]
        public void CalcularValor_AntesTermino_DeveCalcularComMultaCorretamente()
        {
            // Arrange
            var locacao = LocacaoFixture.Create(Plano.A);
            locacao.PrevisaoDevolucao = DateTime.Now.AddDays(4);

            // Act
            var valorCalculado = CalculoValorLocacaoUtility.CalcularValor(locacao);

            // Assert
            Assert.Equal(138, valorCalculado);
        }

        [Fact]
        public void CalcularValor_AposTermino_DeveCalcularComMultaCorretamente()
        {
            // Arrange
            var locacao = LocacaoFixture.Create(Plano.A);
            locacao.PrevisaoDevolucao = DateTime.Now.AddDays(9);

            // Act
            var valorCalculado = CalculoValorLocacaoUtility.CalcularValor(locacao);

            // Assert
            Assert.Equal(310, valorCalculado);
        }
    }
}
