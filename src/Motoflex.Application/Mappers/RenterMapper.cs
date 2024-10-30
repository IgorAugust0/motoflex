using Motoflex.Application.DTOs.Requests;
using Motoflex.Domain.Entities;

namespace Motoflex.Application.Mappers
{
    public static class RenterMapper
    {
        public static Renter Map(this CreateRenter request)
        {
            return new Renter(
                request.Name,
                request.Cnpj,
                request.BirthDate,
                request.Cnh,
                request.CnhType
                );
        }
    }
}
