using MicroRabbit.Domain.Core.Interfaces;
using MicroRabbit.Domain.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MicroRabbit.Data.Repository
{
    public class Repository<T, UpdateTRequest> : IRepository<T, UpdateTRequest>
        where T : BaseModel
        where UpdateTRequest : UpdateBaseRequest
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public IEnumerable<T>? GetAll(Expression<Func<T, bool>> filter = null,
                                      Expression<Func<IQueryable<T>, IOrderedQueryable<T>>> orderBy = null,
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
                query = orderBy.Compile()(query);
            }

            if (skip.HasValue)
            {
                query = query.Skip(skip.Value);
            }

            if (top.HasValue)
            {
                query = query.Take(top.Value);
            }

            return query.AsEnumerable<T>();
        }

        public async Task<T?> GetByIdAsync(int id, params string[]? includeProperties)
        {
            IQueryable<T> query = _dbSet;

            if (includeProperties?.Length > 0)
            {
                query = includeProperties.Aggregate(query, (theQuery, theInclude) => theQuery.Include(theInclude));
            }

            var response = await query.FirstOrDefaultAsync(x => x.Id == id);

            return response;
        }

        public async Task<IEnumerable<T>?> GetByIdsAsync(IEnumerable<int>? ids)
        {
            if (ids == null)
            {
                return null;
            }

            var responseEntities = new List<T>();
            foreach (var id in ids)
            {
                var entity = await GetByIdAsync(id);
                if (entity != null)
                {
                    responseEntities.Add(entity);
                }
            }

            if (responseEntities.Count == 0)
            {
                return null;
            }

            return responseEntities;
        }

        public async Task<bool> IsExistByIdAsync(int id)
        {
            var response = await _dbSet.AnyAsync(e => e.Id == id);
            return response;
        }

        public async Task<bool> IsExistAsync(Expression<Func<T, bool>> filter)
        {
            var response = await _dbSet.AnyAsync(filter);
            return response;
        }

        public async Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter)
        {
            var response = await _dbSet.FirstOrDefaultAsync(filter);
            return response;
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public void AddRange(IEnumerable<T>? entities)
        {
            if (entities == null) { return; }

            _dbSet.AddRange(entities);
        }

        public void Update(T currentEntity, UpdateTRequest updateTRequest)
        {
            updateTRequest.LastUpdatedDate = DateTime.Now;

            //CurrentValues.SetValues only updates scalar properties but not related entities
            _context.Entry(currentEntity).CurrentValues.SetValues(updateTRequest);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void DeleteRange(IEnumerable<T>? entities)
        {
            if (entities == null) { return; }

            _dbSet.RemoveRange(entities);
        }

        public void DeleteRange(Expression<Func<T, bool>> filter)
        {
            var entities = _dbSet.Where(filter);
            if (entities == null) { return; }

            _dbSet.RemoveRange(entities);
        }

        public async Task<int> SaveChangesAsync()
        {
            var response = await _context.SaveChangesAsync();
            return response;
        }
    }
}