using MicroRabbit.Data.Repository;
using MicroRabbit.Orders.Data.Context;
using MicroRabbit.Orders.Domain.Interfaces;
using MicroRabbit.Orders.Domain.Models;

namespace MicroRabbit.Orders.Data.Repository
{
    public class BookRepository : Repository<Book>, IBooksRepository
    {
        private readonly OrderDbContext _context;

        public BookRepository(OrderDbContext context) : base(context)
        {
            _context = context;
        }
    }
}