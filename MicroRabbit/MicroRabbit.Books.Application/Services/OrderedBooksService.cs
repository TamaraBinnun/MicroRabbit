using AutoMapper;
using MicroRabbit.Application.Services;
using MicroRabbit.Books.Application.Dtos.OrderedBooks;
using MicroRabbit.Books.Application.Interfaces;
using MicroRabbit.Books.Domain.Interfaces;
using MicroRabbit.Books.Domain.Models;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Domain.Core.Dtos;

namespace MicroRabbit.Books.Application.Services
{
    public class OrderedBooksService : Service<OrderedBook, OrderedBookResponse, AddOrderedBookRequest, UpdateOrderedBookRequest>, IOrderedBooksService
    {
        private readonly IOrderedBooksRepository _repository;
        private readonly IEventBus _eventBus;
        private readonly IMapper _mapper;

        public OrderedBooksService(IOrderedBooksRepository repository, IEventBus eventBus, IMapper mapper)
            : base(repository, eventBus, mapper)
        {
            _repository = repository;
            _eventBus = eventBus;
            _mapper = mapper;
        }

        public async Task UseEventToUpdateOrderedBooksAsync(IEnumerable<CommonOrderedBook> commonOrderedBooks)
        {
            foreach (var commonOrderedBook in commonOrderedBooks)
            {
                var existEntity = await _repository.GetFirstOrDefaultAsync(filter: b => b.BookId == commonOrderedBook.BookId && b.OrderId == commonOrderedBook.OrderId && b.OrderItemId == commonOrderedBook.OrderItemId);

                if (commonOrderedBook.IsDeleted)
                {
                    if (existEntity == null)
                    {
                        Console.WriteLine($"The ordered book does not exists -  no data to delete");
                    }
                    else
                    {
                        //delete
                        var deletedResponse = await DeleteAsync(existEntity.Id);
                    }
                }
                else
                {
                    if (existEntity == null)
                    {
                        //ordered book does not exists -  add it
                        var addRequest = _mapper.Map<AddOrderedBookRequest>(commonOrderedBook);
                        var addedResponse = await AddAsync(addRequest);
                    }
                    else
                    {
                        //update existing data
                        var updateRequest = _mapper.Map<UpdateOrderedBookRequest>(commonOrderedBook);
                        updateRequest.Id = existEntity.Id;
                        var updatedResponse = await UpdateAsync(updateRequest);
                    }
                }
            }
        }
    }
}