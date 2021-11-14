using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IRepository<T> where T : class, new()
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task<T> DeleteAsync(int id);
        Task<IEnumerable<T>> GetWhereAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetWhereIncludeAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> include);
        Task<IEnumerable<T>> GetAllIncludeAsync(Expression<Func<T, object>> include);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
        Task<T> SingleAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetWithInclude(Expression<Func<T, object>> include);
    }
}
