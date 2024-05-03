using Application.Requests;
using Domain.Entities;

namespace Application.Mappers
{
    public static class RentalMapper
    {
        public static Rental Mapper(this CreateRental request)
        {
            return new Rental(
                request.Plan,
                Guid.Empty
            );
        }
    }
}
