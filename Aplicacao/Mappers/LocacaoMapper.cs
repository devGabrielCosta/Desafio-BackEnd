using Aplicacao.Requests;
using Dominio.Entities;

namespace Aplicacao.Mappers
{
    public static class LocacaoMapper
    {
        public static Locacao Mapper(this CreateLocacao request)
        {
            return new Locacao(
                request.Plano,
                request.EntregadorId
            );
        }
    }
}
