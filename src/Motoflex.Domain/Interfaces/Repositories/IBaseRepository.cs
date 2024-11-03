﻿using Motoflex.Domain.Entities;

namespace Motoflex.Domain.Interfaces.Repositories
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        // IQueryable<T> Get(Guid id);
        // IQueryable<T> Get();

        Task<IQueryable<T>> GetAsync();
        Task<IQueryable<T>> GetByIdAsync(Guid id);

        Task InsertAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task DeleteAsync(Guid entity);
    }
}
