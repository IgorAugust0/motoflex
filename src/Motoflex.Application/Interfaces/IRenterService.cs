using Motoflex.Domain.Entities;

namespace Motoflex.Application.Interfaces
{
    public interface IRenterService
    {
        IEnumerable<Renter> Get();
        Renter? Get(Guid Id);
        Renter? GetRentals(Guid Id);
        Task InsertRenterAsync(Renter renter);
        Task<Renter?> UpdateRenterCnhImageAsync(Guid id, Domain.Utilities.File uploadImage);

        // Task<IEnumerable<Renter>> GetAsync();
        // Task<Renter?> GetByIdAsync(Guid id);
        // Task<Renter?> GetRentalsAsync(Guid id);
        // Task<bool> InsertRenterAsync(Renter renter);
        
    }
}
