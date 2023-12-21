using System.Linq.Expressions;

namespace MicroRabbit.Domain.Core.Interfaces
{
    public interface IReadRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<T?> GetByIdAsync(int id);

        Task<IEnumerable<T>?> GetManyByIdAsync(IEnumerable<int> ids);

        Task<bool> IsExistByIdAsync(int id);

        Task<bool> IsExistAsync(Expression<Func<T, bool>> filter);

        Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter);

        IEnumerable<T>? GetMany(Expression<Func<T, bool>> filter = null,
                                          Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                          int? top = null,
                                          int? skip = null,
                                          params string[] includeProperties);
    }
}