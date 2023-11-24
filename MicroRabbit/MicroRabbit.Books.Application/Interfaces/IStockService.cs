using MicroRabbit.Application.Interfaces;
using MicroRabbit.Books.Application.Dtos;
using MicroRabbit.Books.Domain.Models;
using MicroRabbit.Domain.Core.Dtos;

namespace MicroRabbit.Books.Application.Interfaces
{
    public interface IStockService : IService<StockResponse, BookUnits, UpdateStockRequest>
    {
        Task<IEnumerable<BookUnits>> GetBookUnitsInStockAsync(List<int> bookIds);

        Task<int> UpdateBookUnitsInStockAsync(List<BookUnits> bookUnits);
    }
}