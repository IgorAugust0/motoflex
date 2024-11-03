using Motoflex.Domain.Entities;

namespace Motoflex.Application.Interfaces
{
    public interface IRenterService
    {
        Task<IEnumerable<Renter>> GetAsync();
        Task<Renter?> GetByIdAsync(Guid id);
        Task<Renter?> GetRentalsAsync(Guid id); // GetRenterByIdAsync
        Task<bool> InsertRenterAsync(Renter renter);
        Task<Renter?> UpdateRenterCnhImageAsync(Guid id, Domain.Utilities.File uploadImage);
    }
}
