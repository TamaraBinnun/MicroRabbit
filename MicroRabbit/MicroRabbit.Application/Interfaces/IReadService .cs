namespace MicroRabbit.Application.Interfaces
{
    public interface IReadService<TResponse>
        where TResponse : class
    {
        Task<IEnumerable<TResponse>> GetAllAsync();

        Task<TResponse?> GetByIdAsync(int id);

        Task<IEnumerable<TResponse>?> GetManyByIdAsync(IEnumerable<int> ids);

        Task<bool> IsExistAsync(int id);
    }
}