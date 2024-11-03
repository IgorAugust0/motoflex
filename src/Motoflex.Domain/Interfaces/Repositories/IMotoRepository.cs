using Motoflex.Domain.Entities;

namespace Motoflex.Domain.Interfaces.Repositories
{
    public interface IMotoRepository : IBaseRepository<Motorcycle>
    {
        Task<IEnumerable<Motorcycle>> GetByLicensePlateAsync(string licensePlate);
        Task<IEnumerable<Motorcycle>> GetRentalsAsync();
        Task<Motorcycle?> GetWithRentalsAsync(Guid id);

        // IQueryable<Motorcycle> GetByLicensePlate(string licensePlate);
        // IQueryable<Motorcycle> GetRentals();
    }
}
