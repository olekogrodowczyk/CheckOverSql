using Application.Exceptions;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;
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
            _logger.LogInformation($"New item added to database with type: {typeof(T)}");      

            return entity;
        }

        public async Task<T> DeleteAsync(int id, params Expression<Func<T, object>>[] includeProperties)
        {
            var query = _context.Set<T>();
            includeProperties?.ToList().ForEach(property => query.Include(property));

            var entity = await query.FindAsync(id);
            if (entity == null) { throw new NotFoundException($"Result is not found with id: {id}"); }
            query.Remove(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<T> GetByIdAsync(int id, params Expression<Func<T, object>>[] includeProperties)
        {
            includeProperties?.ToList().ForEach(property => _context.Set<T>().Include(property));

            var result =  await _context.Set<T>().FindAsync(id);
            if(result == null) { throw new NotFoundException($"Result is not found with id:{id} with given type: {typeof(T)}"); }
            return result;
        }

        public async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includeProperties)
        {
            includeProperties?.ToList().ForEach(property => _context.Set<T>().Include(property));

            return await _context.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetWhereAsync(Expression<Func<T, bool>> predicate, 
            params Expression<Func<T, object>>[] includeProperties)
        {
            var query = _context.Set<T>();
            includeProperties?.ToList().ForEach(property => query.Include(property));

            return await query.Where(predicate).ToListAsync();
        }       

        public async Task UpdateAsync(T entity)
        {
            var result = _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            return await _context.Set<T>().AnyAsync(predicate);
        }

        public async Task<T> SingleAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            includeProperties?.ToList().ForEach(property => _context.Set<T>().Include(property));

            var result = await _context.Set<T>().SingleOrDefaultAsync(predicate);
            if (result == null) { throw new NotFoundException("Entity within single method in repository cannot be found"); }
            return result;
        }

        public async Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            includeProperties?.ToList().ForEach(property => _context.Set<T>().Include(property));

            var result = await _context.Set<T>().SingleOrDefaultAsync(predicate);
            return result;
        }
    }
}
