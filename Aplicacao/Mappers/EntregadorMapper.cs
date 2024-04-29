using Aplicacao.Requests;
using Dominio.Entities;

namespace Aplicacao.Mappers
{
    public static class EntregadorMapper
    {
        public static Entregador Mapper(this CreateEntregador request)
        {
            return new Entregador(
                request.Nome,
                request.Cnpj,
                request.DataNascimento,
                request.Cnh,
                request.CnhTipo
            );
        }
    }
}
