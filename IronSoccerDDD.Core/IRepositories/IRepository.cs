using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IronSoccerDDD.Core.IRepositories
{
    public interface IRepository<T, Tkey>
       where T : class
    {
        Task<T> GetByIdAsync(Tkey id);
        Task<T> GetAsync(Expression<Func<T, bool>> filter, string[] includeProperties = null);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task RemoveAsync(T entity);
        Task<IReadOnlyList<T>> GetListAsync(Expression<Func<T, bool>> filter = null, string[] includes = null);
        Task<bool> ExistAsync(Expression<Func<T, bool>> filter = null);
        Task<int> CountAsync(Expression<Func<T, bool>> filter = null);
    }
}
