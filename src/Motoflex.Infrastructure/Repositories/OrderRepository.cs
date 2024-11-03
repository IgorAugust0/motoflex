using Microsoft.EntityFrameworkCore;
using Motoflex.Domain.Entities;
using Motoflex.Domain.Interfaces.Repositories;
using Motoflex.Infrastructure.Contexts;

namespace Motoflex.Infrastructure.Repositories
{
    public class OrderRepository(AppDbContext context) : BaseRepository<Order>(context), IOrderRepository
    {
        public async Task<Order?> GetNotifiedOrdersAsync(Guid id)
        {
            return await _context.Set<Order>()
                .Where(p => p.Id == id)
                .Include(n => n.NotifiedRenters)
                .FirstOrDefaultAsync();
        }
    }
}
