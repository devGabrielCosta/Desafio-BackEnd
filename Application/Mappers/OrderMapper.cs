using Application.Requests;
using Domain.Entities;

namespace Application.Mappers
{
    public static class OrderMapper
    {
        public static Order Mapper(this CreateOrder request)
        {
            return new Order(
                request.DeliveryFee
            );
        }
    }
}
