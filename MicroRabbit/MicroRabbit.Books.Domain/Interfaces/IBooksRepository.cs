using MicroRabbit.Books.Domain.Models;
using MicroRabbit.Domain.Core.Dtos;
using MicroRabbit.Domain.Core.Interfaces;

namespace MicroRabbit.Books.Domain.Interfaces
{
    public interface IBooksRepository : IRepository<Book>
    {
    }
}