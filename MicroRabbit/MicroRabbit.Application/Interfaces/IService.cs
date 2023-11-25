namespace MicroRabbit.Application.Interfaces
{
    public interface IService<TResponse, AddTRequest, UpdateTRequest> : IReadService<TResponse>
        where TResponse : class
        where AddTRequest : class
        where UpdateTRequest : class
    {
        Task<TResponse> AddAsync(AddTRequest addRequest);

        Task<int> UpdateAsync(UpdateTRequest updateRequest);

        Task<int> DeleteAsync(int id);
    }
}