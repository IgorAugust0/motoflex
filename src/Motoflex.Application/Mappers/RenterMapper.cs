using Motoflex.Application.DTOs.Requests;
using Motoflex.Domain.Entities;

namespace Motoflex.Application.Mappers
{
    public static class RenterMapper
    {
        public static Renter Map(this CreateRenter request)
            => new(
                request.Name,
                request.Cnpj,
                request.BirthDate,
                request.Cnh,
                request.CnhType
                );
    }
}
