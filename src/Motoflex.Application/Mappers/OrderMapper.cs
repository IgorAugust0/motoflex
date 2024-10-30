using Motoflex.Application.DTOs.Requests;
using Motoflex.Domain.Entities;

namespace Motoflex.Application.Mappers
{
    public static class OrderMapper
    {
        public static Order Map(this CreateOrder request)
        {
            return new Order(request.DeliveryFee);
        }
    }
}
