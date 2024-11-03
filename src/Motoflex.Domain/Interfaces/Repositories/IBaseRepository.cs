using Motoflex.Domain.Entities;

namespace Motoflex.Domain.Interfaces.Repositories
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        IQueryable<T> Get(Guid id);
        IQueryable<T> Get();

        //Task<IQueryable<T>> GetAsync(Guid id);
        //Task<IQueryable<T>> GetAsync();

        Task InsertAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task DeleteAsync(Guid entity);
    }
}
