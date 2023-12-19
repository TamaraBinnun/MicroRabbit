using MicroRabbit.Domain.Core.Dtos;
using MicroRabbit.Domain.Core.Interfaces;
using MicroRabbit.Domain.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MicroRabbit.Data.Repository
{
    public class ReadRepository<T> : IReadRepository<T>
        where T : BaseModel
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet;

        public ReadRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>?> GetManyByIdAsync(IEnumerable<int> ids)
        {
            var tasks = ids.Select(async id => await GetByIdAsync(id));
            var entities = await Task.WhenAll(tasks);
            var response = entities?.ToList().Where(x => x != null).Select(x => x!);
            return response;
        }

        public async Task<bool> IsExistAsync(int id)
        {
            return await _dbSet.AnyAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> filter = null,
                                            Func<IQueryable<T>,
                                            IOrderedQueryable<T>> orderBy = null,
                                            int? top = null,
                                            int? skip = null,
                                            params string[] includeProperties)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties.Length > 0)
            {
                query = includeProperties.Aggregate(query, (theQuery, theInclude) => theQuery.Include(theInclude));
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (skip.HasValue)
            {
                query = query.Skip(skip.Value);
            }

            if (top.HasValue)
            {
                query = query.Take(top.Value);
            }

            return await query.ToListAsync();
        }
    }
}