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
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(int id);
        Task<T> Add(T entity);
        Task Update(T entity);
        Task<T> Delete(int id);
        Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetWhereInclude(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> include);
        Task<IEnumerable<T>> GetAllInclude(Expression<Func<T, object>> include);
        Task<bool> Exists(Expression<Func<T, bool>> predicate);
    }
}
