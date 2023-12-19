using MicroRabbit.Books.Data.Context;
using MicroRabbit.Books.Domain.Interfaces;
using MicroRabbit.Books.Domain.Models;
using MicroRabbit.Data.Repository;
using MicroRabbit.Domain.Core.Dtos;
using Microsoft.EntityFrameworkCore;

namespace MicroRabbit.Books.Data.Repository
{
    public class OrderedBooksRepository : Repository<OrderedBook>, IOrderedBooksRepository
    {
        private readonly BookDbContext _context;

        public OrderedBooksRepository(BookDbContext context) : base(context)
        {
            _context = context;
        }
    }
}