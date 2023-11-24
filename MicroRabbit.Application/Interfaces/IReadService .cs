namespace MicroRabbit.Application.Interfaces
{
    public interface IReadService<TResponse>
        where TResponse : class
    {
        Task<IEnumerable<TResponse>> GetAllAsync();

        Task<TResponse?> GetByIdAsync(int id);
    }
}