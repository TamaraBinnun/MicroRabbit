using AutoMapper;
using MicroRabbit.Application.Services;
using MicroRabbit.Books.Application.Dtos;
using MicroRabbit.Books.Application.Interfaces;
using MicroRabbit.Books.Domain.Interfaces;
using MicroRabbit.Books.Domain.Models;
using MicroRabbit.Domain.Core.Bus;

namespace MicroRabbit.Books.Application.Services
{
    public class BookService : Service<Book, BookResponse, AddBookRequest, UpdateBookRequest>, IBookService
    {
        private readonly IBookRepository _repository;
        private readonly IEventBus _eventBus;
        private readonly IMapper _mapper;

        public BookService(IBookRepository repository,
                           IEventBus eventBus,
                           IMapper mapper)
            : base(repository, eventBus, mapper)
        {
            _repository = repository;
            _eventBus = eventBus;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Book>> GetTopByPublicationIdAsync(int top, int publicationId)
        {
            return await _repository.GetManyAsync(
                filter: b => b.PublicationId == publicationId,
                orderBy: b => b.OrderBy(b => b.Title),
                top: top);
        }
    }
}