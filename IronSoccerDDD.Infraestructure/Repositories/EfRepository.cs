using IronSoccerDDD.Core.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IronSoccerDDD.Infraestructure.Repositories
{
    public class EfRepository<T, Tkey> : IRepository<T, Tkey>
        where T : class
    {
        protected readonly ApplicationDbContext _context;

        public EfRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public virtual async Task AddAsync(T entity)
        {
            await _context.AddAsync<T>(entity);
        }

        public virtual async Task<int> CountAsync(Expression<Func<T, bool>> filter = null)
        {
            if (filter == null)
            {
                return await _context.Set<T>().CountAsync();
            }

            return await _context.Set<T>().CountAsync(filter);
        }

        public virtual async Task<bool> ExistAsync(Expression<Func<T, bool>> filter = null)
        {
            if (filter == null)
            {
                return await _context.Set<T>().AnyAsync();
            }

            return await _context.Set<T>().AnyAsync(filter);
        }

        public virtual async Task<T> GetAsync(Expression<Func<T, bool>> filter, string[] includeProperties = null)
        {
            if (filter == null)
            {
                throw new ArgumentNullException();
            }

            if (includeProperties == null)
            {
                return await _context.Set<T>().FirstOrDefaultAsync(filter);
            }

            IQueryable<T> query = _context.Set<T>();

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return await query.FirstOrDefaultAsync(filter);
        }
        public virtual async Task<IReadOnlyList<T>> GetListAsync(Expression<Func<T, bool>> filter = null, string[] includeProperties = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }

            if (filter == null)
            {
                return await query.ToListAsync();
            }

            return await query.Where(filter).ToListAsync();
        }

        public virtual async Task<T> GetByIdAsync(Tkey id) => await _context.Set<T>().FindAsync(id);

#pragma warning disable 1998
        // disable async warning
        public virtual async Task RemoveAsync(T entity)
        {
            _context.Remove<T>(entity);
        }
#pragma warning restore 1998

#pragma warning disable 1998
        // disable async warning
        public virtual async Task UpdateAsync(T entity)
        {
            _context.Update<T>(entity);
        }
#pragma warning restore 1998
    }
}
