using MicroRabbit.Application.Interfaces;
using MicroRabbit.Books.Application.Dtos.Books;
using MicroRabbit.Domain.Core.Dtos;

namespace MicroRabbit.Books.Application.Interfaces
{
    public interface IBooksService : IService<BookResponse, AddBookRequest, UpdateBookRequest>
    {
        Task<bool> CreateEventToUpdateBookAsync(CommonBookData bookData);
    }
}