using Dominio.Entities;

namespace Dominio.Utilities
{
    public static class CalculoValorLocacaoUtility
    {
        private const decimal MULTA_APOS_TERMINO = 50;

        public static decimal CalcularValor(Locacao locacao)
        {
            IPlano plano = GetPlano(locacao.Plano);

            var devolucao = locacao.Devolucao.Date;
            var inicioLocacao = locacao.Inicio.Date;
            var terminoLocacao = locacao.Termino.Date;

            if (devolucao < terminoLocacao)
                return CalcularValorAntesDoPrazo(inicioLocacao, terminoLocacao, devolucao, plano);
            else if (devolucao > terminoLocacao)
                return CalcularValorDepoisDoPrazo(inicioLocacao, terminoLocacao, devolucao, plano);
            
            return CalcularValorPrazo(inicioLocacao, terminoLocacao, plano);
        }

        private static decimal CalcularValorPrazo(DateTime inicio, DateTime termino, IPlano plano)
        {
            return ((termino - inicio).Days + 1) * plano.Preco;
        }

        private static decimal CalcularValorAntesDoPrazo(DateTime inicio, DateTime termino, DateTime devolucao, IPlano plano)
        {
            var preco = ((devolucao - inicio).Days + 1) * plano.Preco;
            preco += ((termino - devolucao).Days) * plano.Preco * plano.Multa;
            return preco;
        }

        private static decimal CalcularValorDepoisDoPrazo(DateTime inicio, DateTime termino, DateTime devolucao, IPlano plano)
        {
            var preco = ((termino - inicio).Days + 1) * plano.Preco;
            preco += ((devolucao - termino).Days) * MULTA_APOS_TERMINO;
            return preco;
        }

        private static IPlano GetPlano(Plano plano)
        {
            switch (plano)
            {
                case Plano.A:
                    return new PlanoA();
                case Plano.B:
                    return new PlanoB();
                case Plano.C:
                    return new PlanoC();
                default:
                    throw new ArgumentException("Plano desconhecido", nameof(plano));
            }
        }
    }

}
