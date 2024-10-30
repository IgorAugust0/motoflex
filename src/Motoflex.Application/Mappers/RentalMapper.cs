using Motoflex.Application.DTOs.Requests;
using Motoflex.Domain.Entities;

namespace Motoflex.Application.Mappers
{
    public static class RentalMapper
    {
        public static Rental Map(this CreateRental request)
            => new(request.Plan, Guid.Empty); // id will be set later
    }
}
