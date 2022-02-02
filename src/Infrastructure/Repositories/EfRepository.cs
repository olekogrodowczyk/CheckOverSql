using Application.Common.Exceptions;
using Application.Common.Models;
using Application.Common.Models.ExtenstionMethods;
using Domain.Common;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class EfRepository<T> : IRepository<T> where T : class, new()
    {
        protected readonly ApplicationDbContext _context;
        private readonly ILogger<EfRepository<T>> _logger;

        public EfRepository(ApplicationDbContext context, ILogger<EfRepository<T>> logger)
        {
            _context = context;
            _logger = logger;
        }

        public EfRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<T> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _context.Set<T>().AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var query = _context.Set<T>();
            var entity = await query.FindAsync(id);
            if (entity == null) { throw new NotFoundException($"Result is not found with id: {id}"); }

            query.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var result = await _context.Set<T>().FindAsync(id);
            if (result == null) { throw new NotFoundException($"Result is not found with id:{id} with given type: {typeof(T)}"); }
            return result;
        }

        public async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includeProperties)
        {
            var query = _context.Set<T>().AsQueryable();
            query = includeProperties?.Aggregate(query, (current, include) => current.Include(include));

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetWhereAsync(Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] includeProperties)
        {
            var query = _context.Set<T>().AsQueryable();
            query = includeProperties?.Aggregate(query, (current, include) => current.Include(include));

            return await query.Where(predicate).ToListAsync();
        }

        public async Task<PaginatedList<T>> GetPaginatedResultAsync(Expression<Func<T, bool>> predicate,
             int pageNumber, int pageSize, params Expression<Func<T, object>>[] includeProperties)
        {
            var query = _context.Set<T>().AsQueryable();
            query = includeProperties?.Aggregate(query, (current, include) => current.Include(include));

            return await query.Where(predicate).PaginatedListAsync(pageNumber, pageSize);
        }

        public async Task UpdateAsync(T entity)
        {
            var result = _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().AnyAsync(predicate);
        }

        public async Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            var query = _context.Set<T>().AsNoTracking().AsQueryable();
            query = includeProperties?.Aggregate(query, (current, include) => current.Include(include));

            var result = await query.SingleOrDefaultAsync(predicate);
            return result;
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            var query = _context.Set<T>().AsNoTracking().AsQueryable();
            query = includeProperties?.Aggregate(query, (current, include) => current.Include(include));

            var result = await query.FirstOrDefaultAsync(predicate);
            return result;
        }

        public async Task<int> GetCount(Expression<Func<T, bool>> predicate)
        {
            int result = await _context.Set<T>().Where(predicate).CountAsync();
            return result;
        }
    }
}