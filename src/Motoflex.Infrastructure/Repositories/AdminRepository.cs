using Motoflex.Domain.Entities;
using Motoflex.Domain.Interfaces.Repositories;
using Motoflex.Infrastructure.Contexts;

namespace Motoflex.Infrastructure.Repositories
{
    public class AdminRepository(AppDbContext context) : BaseRepository<Admin>(context), IAdminRepository { }
}
