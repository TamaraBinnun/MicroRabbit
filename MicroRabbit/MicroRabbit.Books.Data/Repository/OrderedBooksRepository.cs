using MicroRabbit.Books.Data.Context;
using MicroRabbit.Books.Domain.Interfaces;
using MicroRabbit.Books.Domain.Models;
using MicroRabbit.Data.Repository;
using MicroRabbit.Domain.Core.Models;

namespace MicroRabbit.Books.Data.Repository
{
    public class OrderedBooksRepository<UpdateTRequest> : Repository<OrderedBook, UpdateTRequest>,
                                                          IOrderedBooksRepository<UpdateTRequest>
        where UpdateTRequest : UpdateBaseRequest
    {
        private readonly BookDbContext _context;

        public OrderedBooksRepository(BookDbContext context) : base(context)
        {
            _context = context;
        }
    }
}