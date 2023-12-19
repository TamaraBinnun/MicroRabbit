using System.Linq.Expressions;

namespace MicroRabbit.Application.Interfaces
{
    public interface IService<TResponse, AddTRequest, UpdateTRequest> : IReadService<TResponse>
        where TResponse : class
        where AddTRequest : class
        where UpdateTRequest : class
    {
        Task<TResponse> AddAsync(AddTRequest addRequest);

        Task<IEnumerable<TResponse>> AddManyAsync(IEnumerable<AddTRequest> addRequest);

        Task<int> UpdateAsync(UpdateTRequest updateRequest);

        Task<int> UpdateManyAsync(IEnumerable<UpdateTRequest> updateRequest);

        Task<int> DeleteAsync(int id);

        Task<int> DeleteManyAsync(IEnumerable<int> id);

        Task<int> DeleteManyByFilterAsync(Expression<Func<TResponse, bool>> filter);
    }
}