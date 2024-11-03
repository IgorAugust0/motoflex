using Microsoft.EntityFrameworkCore;
using Motoflex.Domain.Entities;
using Motoflex.Domain.Interfaces.Repositories;
using Motoflex.Infrastructure.Contexts;

namespace Motoflex.Infrastructure.Repositories
{
    public class MotorcycleRepository(AppDbContext context) : BaseRepository<Motorcycle>(context), IMotoRepository
    {
        public async Task<IEnumerable<Motorcycle>> GetByLicensePlateAsync(string licensePlate)
        {
            return await _context.Set<Motorcycle>()
                .Where(m => EF.Functions.Like(m.LicensePlate, $"{licensePlate}%"))
                .ToListAsync();
        }

        public async Task<IEnumerable<Motorcycle>> GetRentalsAsync()
        {
            return await _context.Set<Motorcycle>()
                .Include(m => m.Rentals)
                .ToListAsync();
        }

        public async Task<Motorcycle?> GetWithRentalsAsync(Guid id)
        {
            return await _context.Set<Motorcycle>()
                .Include(m => m.Rentals)
                .FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
