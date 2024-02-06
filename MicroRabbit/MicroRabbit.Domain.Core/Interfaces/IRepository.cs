using MicroRabbit.Domain.Core.Models;
using System.Linq.Expressions;

namespace MicroRabbit.Domain.Core.Interfaces
{
    public interface IRepository<T, UpdateTRequest>
        where T : BaseModel
        where UpdateTRequest : UpdateBaseRequest
    {
        IEnumerable<T>? GetAll(Expression<Func<T, bool>> filter = null,
                               Expression<Func<IQueryable<T>, IOrderedQueryable<T>>> orderBy = null,
                                int? top = null,
                                int? skip = null,
                                params string[] includeProperties);

        Task<T?> GetByIdAsync(int id, params string[]? includeProperties);

        Task<IEnumerable<T>?> GetByIdsAsync(IEnumerable<int>? ids);

        Task<bool> IsExistByIdAsync(int id);

        Task<bool> IsExistAsync(Expression<Func<T, bool>> filter);

        Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter);

        void Add(T entity);

        void AddRange(IEnumerable<T>? entities);

        void Update(T currentEntity, UpdateTRequest updateTRequest);

        void Delete(T entity);

        void DeleteRange(IEnumerable<T>? entities);

        void DeleteRange(Expression<Func<T, bool>> filter);

        Task<int> SaveChangesAsync();
    }
}