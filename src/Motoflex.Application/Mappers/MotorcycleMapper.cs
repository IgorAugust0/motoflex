using Motoflex.Application.DTOs.Requests;
using Motoflex.Domain.Entities;

namespace Motoflex.Application.Mappers
{
    // if desired, all these mapper classes could be merged into a single entity mapper class
    public static class MotorcycleMapper
    {
        public static Motorcycle Map(this CreateMotorcycle request)
            // to use null checks, turn method to block body
            // ArgumentNullException.ThrowIfNull(request);
            => new(request.Year, request.Model!, request.LicensePlate!);
    }
}
