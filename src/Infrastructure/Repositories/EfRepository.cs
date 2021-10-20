using Application.Exceptions;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
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

        public EfRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<T> Add(T entity)
        {
            _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<T> Delete(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if(entity == null)
            {
                throw new NotFoundException("Entity not found");
            }
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<T> GetById(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _context.Set<T>()
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllInclude(Expression<Func<T, object>> include)
        {
            return await _context.Set<T>()
                .Include(include)
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>()
                .Where(predicate)
                .ToListAsync();              
        }

        public async Task<IEnumerable<T>> GetWhereInclude(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> include)
        {
            return await _context.Set<T>()
                .Include(include)
                .Where(predicate)                
                .ToListAsync();
        }

        public async Task Update(T entity)
        {
            var post = _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
