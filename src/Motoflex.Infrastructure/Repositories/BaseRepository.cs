﻿using Microsoft.EntityFrameworkCore;
using Motoflex.Domain.Entities;
using Motoflex.Domain.Interfaces.Repositories;
using Motoflex.Infrastructure.Contexts;

namespace Motoflex.Infrastructure.Repositories
{
    public class BaseRepository<T>(AppDbContext context) : IBaseRepository<T> where T : BaseEntity
    {
        protected readonly AppDbContext _context = context;

        public async Task<IQueryable<T>> GetAsync()
        {
            var result = await _context.Set<T>()
                .AsNoTracking()
                .ToListAsync();
            return result.AsQueryable();
        }

        public virtual async Task<IQueryable<T>> GetByIdAsync(Guid id)
        {
            var result = await _context.Set<T>()
                .Where(x => x.Id == id)
                .AsNoTracking()
                .ToListAsync();
            return result.AsQueryable();
        }

        // public virtual async Task<T?> GetByIdAsync(Guid id) // Changed from IQueryable to T
        // {
        //     return await _context.Set<T>()
        //         .AsNoTracking()
        //         .SingleOrDefaultAsync(x => x.Id == id);
        // }

        public async Task InsertAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity != null) await DeleteAsync(entity);
        }
    }

}
