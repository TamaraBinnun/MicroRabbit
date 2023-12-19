using MicroRabbit.Application.Interfaces;
using MicroRabbit.Books.Application.Dtos.OrderedBooks;
using MicroRabbit.Domain.Core.Dtos;

namespace MicroRabbit.Books.Application.Interfaces
{
    public interface IOrderedBooksService : IService<OrderedBookResponse, AddOrderedBookRequest, UpdateOrderedBookRequest>
    {
        Task UseEventToUpdateOrderedBooksAsync(IEnumerable<CommonOrderedBook> commonOrderedBooks);
    }
}