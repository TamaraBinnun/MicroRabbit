using AutoMapper;
using MicroRabbit.Orders.Application.Dtos;
using MicroRabbit.Orders.Application.Interfaces;
using MicroRabbit.Orders.Domain.Interfaces;
using MicroRabbit.Orders.Domain.Models;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Application.Services;
using MicroRabbit.Domain.Core.Dtos;
using MicroRabbit.Orders.Application.Dtos.Books;

namespace MicroRabbit.Orders.Application.Services
{
    public class BooksService : Service<Book, BookResponse, AddBookRequest, UpdateBookRequest>, IBooksService
    {
        private readonly IBooksRepository _repository;
        private readonly IEventBus _eventBus;
        private readonly IMapper _mapper;

        public BooksService(IBooksRepository repository, IEventBus eventBus, IMapper mapper)
             : base(repository, eventBus, mapper)
        {
            _repository = repository;
            _eventBus = eventBus;
            _mapper = mapper;
        }

        public async Task UseEventToUpdateBookAsync(CommonBookData commonBookData)
        {
            var bookEntity = await _repository.GetFirstOrDefaultAsync(filter: b => b.ExternalId == commonBookData.BookId);
            if (commonBookData.IsDeleted)
            {
                if (bookEntity == null)
                {
                    Console.WriteLine($"Book with externalId:{commonBookData.BookId} is not exists -  no record to delete");
                }
                else
                {
                    await DeleteAsync(bookEntity.Id);
                    Console.WriteLine($"Book id: {bookEntity.Id} has been deleted");
                }
            }
            else
            {
                if (bookEntity == null)
                {
                    var addBookRequest = _mapper.Map<AddBookRequest>(commonBookData);
                    var bookAdded = await AddAsync(addBookRequest);
                    Console.WriteLine($"Book id: {bookAdded.Id}, title: {bookAdded.Title}, external id:{bookAdded.ExternalId} was added.");
                }
                else
                {
                    bookEntity.Title = commonBookData.Title;
                    bookEntity.LastUpdatedDate = DateTime.Now;
                    var updateResponse = await _repository.SaveChangesAsync();

                    if (updateResponse > 0)
                    {
                        Console.WriteLine($"Book id: {bookEntity.Id} was updated");
                    }
                }
            }
        }
    }
}