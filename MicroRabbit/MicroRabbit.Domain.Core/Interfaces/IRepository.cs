using System.Linq.Expressions;

namespace MicroRabbit.Domain.Core.Interfaces
{
    public interface IRepository<T> : IReadRepository<T>
        where T : class
    {
        void Add(T entity);

        void Update(T entity);

        void Delete(T entity);

        void DeleteMany(Expression<Func<T, bool>> filter);

        Task<int> SaveChangesAsync();
    }
}