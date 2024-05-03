using Application.Requests;
using Domain.Entities;

namespace Application.Mappers
{
    public static class CourierMapper
    {
        public static Courier Mapper(this CreateCourier request)
        {
            return new Courier(
                request.Name,
                request.Cnpj,
                request.BirthDate,
                request.Cnh,
                request.CnhType
            );
        }
    }
}
