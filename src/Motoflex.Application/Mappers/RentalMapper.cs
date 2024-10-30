using Motoflex.Application.DTOs.Requests;
using Motoflex.Domain.Entities;

namespace Motoflex.Application.Mappers
{
    public static class RentalMapper
    {
        public static Rental Map(this CreateRental request)
        {
            return new Rental(
                request.Plan,
                Guid.Empty // will be set later
                );
        }
    }
}
