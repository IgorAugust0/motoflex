using Motoflex.Domain.Entities;

namespace Motoflex.Domain.Interfaces.Repositories
{
    public interface IRenterRepository : IBaseRepository<Renter>
    {
        Task<IEnumerable<Renter>> GetAllRentalsAsync();
        Task<IEnumerable<Renter>> GetAvailableRentersForOrderAsync();

        // IQueryable<Renter> GetAllRentals();
        // IQueryable<Renter> GetAvailableRentersForOrder();
    }
}
