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
            var tasks = commonOrderedBooks.Select(async commonOrderedBook =>
            {
                var existEntity = await GetByBookIdOrderIdOrderItemIdAsync(commonOrderedBook.BookId, commonOrderedBook.OrderId, commonOrderedBook.OrderItemId);

                if (commonOrderedBook.IsDeleted)
                {
                    if (existEntity == null)
                    {
                        Console.WriteLine($"The ordered book does not exists -  no data to delete");
                    }
                    else
                    {
                        //delete
                        await DeleteAsync(existEntity.Id);
                    }
                }
                else
                {
                    if (existEntity == null)
                    {
                        //ordered book does not exists -  add it
                        var addRequest = _mapper.Map<AddOrderedBookRequest>(commonOrderedBook);
                        await AddAsync(addRequest);
                    }
                    else
                    {
                        //update existing data
                        var updateRequest = _mapper.Map<UpdateOrderedBookRequest>(commonOrderedBook);
                        updateRequest.Id = existEntity.Id;
                        await UpdateAsync(updateRequest);
                    }
                }
            }).ToList();

            await Task.WhenAll(tasks);
        }

        private async Task<OrderedBook?> GetByBookIdOrderIdOrderItemIdAsync(int bookId, int orderId, int orderItemId)
        {
            var orderedBook = await _repository.GetManyAsync(
                filter: b => b.BookId == bookId && b.OrderId == orderId && b.OrderItemId == orderItemId,
                top: 1);

            if (orderedBook == null)
            {
                return null;
            }

            return orderedBook.ToList().FirstOrDefault();
        }
    }
}