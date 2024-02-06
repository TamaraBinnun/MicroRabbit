using MicroRabbit.Data.Repository;
using MicroRabbit.Domain.Core.Models;
using MicroRabbit.Orders.Data.Context;
using MicroRabbit.Orders.Domain.Interfaces;
using MicroRabbit.Orders.Domain.Models;

namespace MicroRabbit.Orders.Data.Repository
{
    public class BookRepository<UpdateTRequest> : Repository<Book, UpdateTRequest>,
                                                  IBooksRepository<UpdateTRequest>
        where UpdateTRequest : UpdateBaseRequest
    {
        private readonly OrderDbContext _context;

        public BookRepository(OrderDbContext context) : base(context)
        {
            _context = context;
        }
    }
}