using Microsoft.EntityFrameworkCore;
using Motoflex.Domain.Entities;
using Motoflex.Domain.Interfaces.Repositories;
using Motoflex.Infrastructure.Contexts;

namespace Motoflex.Infrastructure.Repositories
{
    public class RentalRepository(AppDbContext context) : BaseRepository<Rental>(context), IRentalRepository
    {
        public override async Task<IQueryable<Rental>> GetByIdAsync(Guid id)
        {
            var result = await _context.Set<Rental>()
                .Include(ra => ra.Motorcycle)
                // .FirstOrDefaultAsync(r => r.Id == id); if just using Task<Rental>
                .Where(x => x.Id == id)
                .AsNoTracking()
                .ToListAsync();
            return result.AsQueryable();
        }
    }
}
