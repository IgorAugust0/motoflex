using Motoflex.Domain.Entities;

namespace Motoflex.Domain.Interfaces.Repositories
{
    public interface IRenterRepository : IBaseRepository<Renter>
    {
        IQueryable<Renter> GetRentals();
        IQueryable<Renter> AvailableRentersForOrder();
    }
}
