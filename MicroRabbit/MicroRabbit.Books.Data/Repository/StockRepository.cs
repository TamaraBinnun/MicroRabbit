using MicroRabbit.Books.Data.Context;
using MicroRabbit.Books.Domain.Interfaces;
using MicroRabbit.Books.Domain.Models;
using MicroRabbit.Data.Repository;
using MicroRabbit.Domain.Core.Dtos;
using Microsoft.EntityFrameworkCore;

namespace MicroRabbit.Books.Data.Repository
{
    public class StockRepository : Repository<BookInStock>, IStockRepository
    {
        private readonly BookDbContext _context;

        public StockRepository(BookDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<BookInStock?> GetByBookIdAsync(int bookId)
        {
            var response = await _context.BooksInStock.FirstOrDefaultAsync(x => x.BookId == bookId);
            return response;
        }
    }
}