using Aplicacao.Requests;
using Dominio.Entities;

namespace Aplicacao.Mappers
{
    public static class MotoMapper
    {
        public static Moto Mapper(this CreateMoto request)
        {
            return new Moto(
                request.Ano,
                request.Modelo,
                request.Placa
            );
        }
    }
}
