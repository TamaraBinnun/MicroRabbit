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
        private readonly IOrderedBooksRepository<UpdateOrderedBookRequest> _repository;
        private readonly IEventBus _eventBus;
        private readonly IMapper _mapper;

        public OrderedBooksService(IOrderedBooksRepository<UpdateOrderedBookRequest> repository, IEventBus eventBus, IMapper mapper)
            : base(repository, eventBus, mapper)
        {
            _repository = repository;
            _eventBus = eventBus;
            _mapper = mapper;
        }

        public async Task<int?> UseEventToUpdateOrderedBooksAsync(CommonOrder commonOrder)
        {
            if (commonOrder == null || commonOrder.Id <= 0)
            {
                return null;
            }

            //fix request: sum OrderedUnits by BookId
            var distinctOrderItemsRequest = commonOrder.OrderItems?.GroupBy(x => x.BookId, x => x,
                (key, values) =>
                {
                    var firstOrderItem = values.First();
                    var sumOrderedUnits = values.Sum(b => b.OrderedUnits);
                    firstOrderItem.OrderedUnits = sumOrderedUnits;
                    return firstOrderItem;
                }).ToList();

            var existedItems = _repository.GetAll(filter: b => b.OrderId == commonOrder.Id);

            var addedItems = await GetAddedItems(commonOrder.Id, existedItems, distinctOrderItemsRequest);
            var updatedItems = GetUpdatedItems(commonOrder.Id, existedItems, distinctOrderItemsRequest);
            var deletedItems = GetDeletedItems(commonOrder.Id, existedItems, distinctOrderItemsRequest);

            var saveResponse = await _repository.SaveChangesAsync();

            return saveResponse;
        }

        private async Task<IEnumerable<OrderedBookResponse>?> GetAddedItems(int orderId, IEnumerable<OrderedBook>? existedItems, IEnumerable<CommonOrderedBook>? orderItemsRequest)
        {
            if (orderItemsRequest == null)
            {//no items to add
                return null;
            }

            List<AddOrderedBookRequest>? itemsToAdd = null;

            if (existedItems == null)
            {//order not exists -> all items from request to add
                itemsToAdd = _mapper.Map<List<AddOrderedBookRequest>>(orderItemsRequest);
            }
            else
            {
                itemsToAdd = (from orderItemRequest in orderItemsRequest
                              join existedItem in existedItems
                              on new { OrderId = orderId, orderItemRequest.OrderItemId, orderItemRequest.BookId }
                              equals new { existedItem.OrderId, existedItem.OrderItemId, BookId = existedItem.ExternalBookId } into temp
                              from l in temp.DefaultIfEmpty()
                              where l is null //no existedItem
                              select _mapper.Map<AddOrderedBookRequest>(orderItemRequest)).ToList();
            }

            itemsToAdd?.ForEach(x => x.OrderId = orderId);
            var addedItems = await AddRangeAsync(itemsToAdd);
            return addedItems;
        }

        private IEnumerable<OrderedBookResponse>? GetUpdatedItems(int orderId, IEnumerable<OrderedBook>? existedItems, IEnumerable<CommonOrderedBook>? orderItemsRequest)
        {
            if (existedItems == null || orderItemsRequest == null) { return null; }//no items to update

            var itemsToUpdate = (from existedItem in existedItems
                                 join orderItemRequest in orderItemsRequest
                                 on new { existedItem.OrderId, existedItem.OrderItemId, BookId = existedItem.ExternalBookId }
                                 equals new { OrderId = orderId, orderItemRequest.OrderItemId, orderItemRequest.BookId }
                                 select new { existedItem, orderItemRequest }).ToList();

            itemsToUpdate?.ForEach(x =>
            {
                //creating updateRequest mapping from two sources:existedItem & orderItemRequest
                var updateRequest = _mapper.Map<UpdateOrderedBookRequest>(x.orderItemRequest);

                _repository.Update(x.existedItem, updateRequest);
            });

            var response = _mapper.Map<IEnumerable<OrderedBookResponse>?>(itemsToUpdate?.Select(x => x.existedItem));
            return response;
        }

        //delete and return deleted items
        private IEnumerable<OrderedBookResponse>? GetDeletedItems(int orderId, IEnumerable<OrderedBook>? existedItems, IEnumerable<CommonOrderedBook>? orderItemsRequest)
        {
            if (existedItems == null) { return null; }//no items to delete

            IEnumerable<OrderedBook>? itemsToDelete;
            if (orderItemsRequest == null)
            {//delete all existedItems items
                itemsToDelete = existedItems;
            }
            else
            {
                itemsToDelete = (from existedItem in existedItems
                                 join orderItemRequest in orderItemsRequest
                                 on new { existedItem.OrderId, existedItem.OrderItemId, BookId = existedItem.ExternalBookId }
                                 equals new { OrderId = orderId, orderItemRequest.OrderItemId, orderItemRequest.BookId } into temp
                                 from l in temp.DefaultIfEmpty()
                                 where l is null //no orderItemRequest
                                 select existedItem);
            }

            _repository.DeleteRange(itemsToDelete);

            var response = _mapper.Map<IEnumerable<OrderedBookResponse>?>(itemsToDelete);
            return response;
        }
    }
}