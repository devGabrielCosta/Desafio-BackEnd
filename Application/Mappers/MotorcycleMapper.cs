using Application.Requests;
using Domain.Entities;

namespace Application.Mappers
{
    public static class MotorcycleMapper
    {
        public static Motorcycle Mapper(this CreateMotorcycle request)
        {
            return new Motorcycle(
                request.Year,
                request.Model,
                request.LicensePlate
            );
        }
    }
}
