using MicroRabbit.Application.Interfaces;
using MicroRabbit.Orders.Application.Dtos;

namespace MicroRabbit.Orders.Application.Interfaces
{
    public interface IBookService : IReadService<BookResponse>
    {
    }
}