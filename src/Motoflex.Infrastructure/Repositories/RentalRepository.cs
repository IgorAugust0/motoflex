using Microsoft.EntityFrameworkCore;
using Motoflex.Domain.Entities;
using Motoflex.Domain.Interfaces.Repositories;
using Motoflex.Infrastructure.Contexts;

namespace Motoflex.Infrastructure.Repositories
{
    public class RentalRepository(AppDbContext context) : BaseRepository<Rental>(context), IRentalRepository
    {
        public override IQueryable<Rental> Get(Guid id) // remove override if something goes wrong
        {
            return base.Get(id).Include(ra => ra.Motorcycle);
        }
    }
}
