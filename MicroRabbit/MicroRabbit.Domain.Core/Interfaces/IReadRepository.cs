using System.Linq.Expressions;

namespace MicroRabbit.Domain.Core.Interfaces
{
    public interface IReadRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<T?> GetByIdAsync(int id);

        Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> filter = null,
                                          Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                          int? top = null,
                                          int? skip = null,
                                          params string[] includeProperties);
    }
}