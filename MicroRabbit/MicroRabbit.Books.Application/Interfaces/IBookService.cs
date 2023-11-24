using MicroRabbit.Application.Interfaces;
using MicroRabbit.Books.Application.Dtos;
using MicroRabbit.Books.Domain.Models;

namespace MicroRabbit.Books.Application.Interfaces
{
    public interface IBookService : IService<BookResponse, AddBookRequest, UpdateBookRequest>
    {
        Task<IEnumerable<Book>> GetTopByPublicationIdAsync(int top, int publicationId);
    }
}