using Motoflex.Domain.Entities;

namespace Motoflex.Domain.Interfaces.Repositories
{
    public interface IMotoRepository : IBaseRepository<Motorcycle>
    {
        IQueryable<Motorcycle> GetByLicensePlate(string licensePlate);
        IQueryable<Motorcycle> GetRentals();
    }
}
