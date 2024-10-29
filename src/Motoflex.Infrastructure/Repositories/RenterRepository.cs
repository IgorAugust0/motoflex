using Microsoft.EntityFrameworkCore;
using Motoflex.Domain.Entities;
using Motoflex.Domain.Interfaces.Repositories;
using Motoflex.Infrastructure.Contexts;

namespace Motoflex.Infrastructure.Repositories
{
    public class RenterRepository(AppDbContext context) : BaseRepository<Renter>(context), IRenterRepository
    {
        public IQueryable<Renter> GetAllRentals()
        {
            return _context.Set<Renter>().Include(ra => ra.Rentals);
        }

        public IQueryable<Renter> GetAvailableRentersForOrder()
        {
            return _context.Set<Renter>()
                .Include(re => re.Rentals) // load rentals for each renter
                .Where(re => re.Rentals.Any(re => re.Active)) // renters with active rentals
                .Include(re => re.Orders) // load orders for filtered renters
                .Where(re => !re.Orders.Any(o => o.Status == Status.Available)); // renter without available orders

            //.Include(re => re.Rentals).Include(re => re.Orders)
            //.Where(re => re.Rentals.Any(r => r.Active) && !re.Orders.Any(o => o.Status == Status.Available));
        }
    }
}
