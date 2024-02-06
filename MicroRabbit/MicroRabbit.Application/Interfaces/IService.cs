using MicroRabbit.Domain.Core.Models;
using System.Linq.Expressions;

namespace MicroRabbit.Application.Interfaces
{
    public interface IService<TResponse, AddTRequest, UpdateTRequest>
        where TResponse : BaseResponse
        where AddTRequest : class
        where UpdateTRequest : UpdateBaseRequest
    {
        IEnumerable<TResponse>? GetAll(Expression<Func<TResponse, bool>> filter = null,
                                       Expression<Func<IQueryable<TResponse>, IOrderedQueryable<TResponse>>> orderBy = null,
                                       int? top = null,
                                       int? skip = null,
                                       params string[] includeProperties);

        Task<TResponse?> GetByIdAsync(int id, params string[] includeProperties);

        Task<IEnumerable<TResponse>?> GetByIdsAsync(IEnumerable<int>? ids);

        Task<bool> IsExistByIdAsync(int id);

        Task<TResponse> AddAsync(AddTRequest addRequest);

        Task<IEnumerable<TResponse>?> AddRangeAsync(IEnumerable<AddTRequest>? addRequest);

        Task<TResponse?> UpdateAsync(int id, UpdateTRequest updateRequest, params string[] includeProperties);

        Task<TResponse?> DeleteAsync(int id);

        Task<IEnumerable<TResponse>?> DeleteRange(IEnumerable<int>? id);

        Task<IEnumerable<TResponse>?> DeleteManyByFilterAsync(Expression<Func<TResponse, bool>> filter);
    }
}