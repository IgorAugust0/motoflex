using Motoflex.Domain.Entities;

namespace Motoflex.Application.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAsync();
        Task<Order?> GetNotifiedOrdersAsync(Guid id);
        Task<Order?> InsertOrderAsync(Order order);
        Task<bool> AcceptOrderAsync(Guid id, Guid renterId);
        Task<bool> FinishOrderAsync(Guid id, Guid renterId);
    }
}
