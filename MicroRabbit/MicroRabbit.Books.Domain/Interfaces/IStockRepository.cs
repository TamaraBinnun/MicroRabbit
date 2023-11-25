using MicroRabbit.Books.Domain.Models;
using MicroRabbit.Domain.Core.Interfaces;

namespace MicroRabbit.Books.Domain.Interfaces
{
    public interface IStockRepository : IRepository<BookInStock>
    {
        Task<BookInStock?> GetByBookIdAsync(int bookId);
    }
}