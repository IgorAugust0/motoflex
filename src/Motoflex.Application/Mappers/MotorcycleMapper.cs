using Motoflex.Application.DTOs.Requests;
using Motoflex.Domain.Entities;

namespace Motoflex.Application.Mappers
{
    public static class MotorcycleMapper
    {
        public static Motorcycle Map(this CreateMotorcycle request)
        {
            // or add null checks, instead of using ! operator
            return new Motorcycle(request.Year, request.Model!, request.LicensePlate!);
        }
    }
}
