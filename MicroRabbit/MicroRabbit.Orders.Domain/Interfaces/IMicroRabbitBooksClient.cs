using MicroRabbit.Domain.Core.Dtos;

namespace MicroRabbit.Orders.Domain.Interfaces
{
    public interface IMicroRabbitBooksClient
    {
        Task<IEnumerable<BookUnits>?> GetBookUnitsInStockAsync(List<int> bookIds);

        Task<bool> UpdateBookUnitsInStockAsync(List<BookUnits> bookUnits);
    }
}