using Motoflex.Domain.Entities;

namespace Motoflex.Domain.Interfaces.Repositories
{
    public interface IMotoRepository : IBaseRepository<Motorcycle>
    {
        IQueryable<Motorcycle> GetByLicensePlate(string licensePlate);
        IQueryable<Motorcycle> GetRentals();

        //Task<IQueryable<Motorcycle>> GetByLicensePlateAsync(string licensePlate);
        //Task<IQueryable<Motorcycle>> GetRentalsAsync();
    }
}
