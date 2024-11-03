using Motoflex.Domain.Entities;

namespace Motoflex.Application.Interfaces
{
    public interface IRenterService
    {
        IEnumerable<Renter> Get();
        // Task<IEnumerable<Renter>> GetAsync();
        Renter? Get(Guid Id);
        // Task<Renter?> GetByIdAsync(Guid id);
        // Renter? GetRentals(Guid Id);
        Task<Renter?> GetRentalsAsync(Guid id);
        Task<bool> InsertRenterAsync(Renter renter);
        Task<Renter?> UpdateRenterCnhImageAsync(Guid id, Domain.Utilities.File uploadImage);
    }
}
