using Microsoft.EntityFrameworkCore;
using Motoflex.Domain.Entities;
using Motoflex.Domain.Interfaces.Repositories;
using Motoflex.Infrastructure.Contexts;

namespace Motoflex.Infrastructure.Repositories
{
    public class MotorcycleRepository(AppDbContext context) : BaseRepository<Motorcycle>(context), IMotoRepository
    {
        public IQueryable<Motorcycle> GetByLicensePlate(string licensePlate)
        {
            return _context.Set<Motorcycle>()
                .Where(m => EF.Functions.Like(m.LicensePlate, $"{licensePlate}%"));
        }

        public IQueryable<Motorcycle> GetRentals()
        {
            return _context.Set<Motorcycle>()
                .Include(m => m.Rentals); // nameof(Motorcycle.Rentals)
        }
    }
}
