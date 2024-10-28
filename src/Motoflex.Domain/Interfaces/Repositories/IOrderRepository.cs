using Motoflex.Domain.Entities;

namespace Motoflex.Domain.Interfaces.Repositories
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        IQueryable<Order> GetNotifiedOrders(Guid id);
    }
}
