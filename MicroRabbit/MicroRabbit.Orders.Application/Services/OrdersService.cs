using AutoMapper;
using MicroRabbit.Application.Services;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Domain.Core.Dtos;
using MicroRabbit.Domain.Core.Interfaces;
using MicroRabbit.Orders.Application.Dtos.OrderItems;
using MicroRabbit.Orders.Application.Dtos.Orders;
using MicroRabbit.Orders.Application.Interfaces;
using MicroRabbit.Orders.Domain.Commands;
using MicroRabbit.Orders.Domain.Interfaces;
using MicroRabbit.Orders.Domain.Models;
using Microsoft.Extensions.Configuration;

namespace MicroRabbit.Orders.Application.Services
{
    public class OrdersService : Service<Order, OrderResponse, AddOrderRequest, UpdateOrderRequest>, IOrdersService
    {
        private readonly IOrdersRepository<UpdateOrderRequest, UpdateOrderItemRequest> _orderRepository;
        private readonly IEventBus _eventBus;
        private readonly IMapper _mapper;
        private readonly ISynchronousSender _synchronousSender;
        private readonly IConfiguration _config;

        public OrdersService(IOrdersRepository<UpdateOrderRequest, UpdateOrderItemRequest> orderRepository,
                             IEventBus eventBus,
                             IMapper mapper,
                                 ISynchronousSender synchronousSender,
                                 IConfiguration config) : base(orderRepository, eventBus, mapper)
        {
            _orderRepository = orderRepository;
            _eventBus = eventBus;
            _mapper = mapper;
            _synchronousSender = synchronousSender;
            _config = config;
        }

        public async Task<OrderResponse?> UpdateOrder(int id, UpdateOrderRequest updateOrderRequest)
        {
            var existedOrder = await _orderRepository.GetByIdAsync(id, includeProperties: new string[] { "OrderItems" });
            if (existedOrder == null)
            {
                return null;
            }

            updateOrderRequest.OrderStatus = OrderStatus.Updated;
            _orderRepository.Update(existedOrder, updateOrderRequest);//not updates order items
            UpdateOrderItems(existedOrder.Id, existedOrder.OrderItems, updateOrderRequest.OrderItems);

            var saveOrderResponse = await _orderRepository.SaveChangesAsync();

            var response = _mapper.Map<OrderResponse>(existedOrder);//entity with the updated data

            return response;
        }

        private void UpdateOrderItems(int orderId, IEnumerable<OrderItem>? existedItems, IEnumerable<AddOrderItemRequest>? orderItemsRequest)
        {
            //fix request: sum OrderedUnits by BookId
            var distinctOrderItemsRequest = orderItemsRequest?.GroupBy(x => x.BookId, x => x,
                 (key, values) =>
                 {
                     var firstOrderItem = values.First();
                     firstOrderItem.OrderedUnits = values.Sum(b => b.OrderedUnits);
                     return firstOrderItem;
                 }).ToList();

            var itemsToUpdate = GetItemsToUpdate(existedItems, distinctOrderItemsRequest);
            _orderRepository.UpdateOrderItems(itemsToUpdate);

            var itemsToAdd = GeItemsToAdd(existedItems, distinctOrderItemsRequest);
            _orderRepository.AddOrderItemRange(itemsToAdd);

            var itemsToDelete = GetItemsToDelete(existedItems, distinctOrderItemsRequest);
            _orderRepository.DeleteOrderItemRange(itemsToDelete);
        }

        private IEnumerable<OrderItem>? GeItemsToAdd(IEnumerable<OrderItem>? existedItems, IEnumerable<AddOrderItemRequest>? orderItemsRequest)
        {
            if (orderItemsRequest == null)
            {//no items to add
                return null;
            }

            IEnumerable<AddOrderItemRequest>? itemsToAdd = null;

            if (existedItems == null)
            {//all items from request to add
                itemsToAdd = orderItemsRequest;
            }
            else
            {
                itemsToAdd = (from orderItemRequest in orderItemsRequest
                              join existedItem in existedItems
                              on orderItemRequest.BookId equals existedItem.BookId into temp
                              from l in temp.DefaultIfEmpty()
                              where l is null //no existedItem
                              select orderItemRequest);
            }

            var response = _mapper.Map<IEnumerable<OrderItem>>(itemsToAdd);
            return response;
        }

        private IEnumerable<(OrderItem currentOrderItem, UpdateOrderItemRequest updateOrderItemRequest)>? GetItemsToUpdate(IEnumerable<OrderItem>? existedItems, IEnumerable<AddOrderItemRequest>? orderItemsRequest)
        {
            if (existedItems == null || orderItemsRequest == null) { return null; }//no items to update

            var itemsToUpdate = (from existedItem in existedItems
                                 join orderItemRequest in orderItemsRequest
                                 on existedItem.BookId equals orderItemRequest.BookId
                                 select
                                 (
                                    currentOrderItem: existedItem,
                                    updateOrderItemRequest: _mapper.Map<UpdateOrderItemRequest>(orderItemRequest)
                                 ));

            return itemsToUpdate;
        }

        //delete and return deleted items
        private IEnumerable<OrderItem>? GetItemsToDelete(IEnumerable<OrderItem>? existedItems, IEnumerable<AddOrderItemRequest>? orderItemsRequest)
        {
            if (existedItems == null) { return null; }

            IEnumerable<OrderItem>? itemsToDelete;
            if (orderItemsRequest == null)
            {
                itemsToDelete = existedItems;
            }
            else
            {
                itemsToDelete = (from existedItem in existedItems
                                 join orderItemRequest in orderItemsRequest
                                 on existedItem.BookId equals orderItemRequest.BookId into temp
                                 from l in temp.DefaultIfEmpty()
                                 where l is null //no orderItemRequest
                                 select existedItem);
            }

            return itemsToDelete;
        }

        public async Task<bool> CreateEventToUpdateOrderedBooksAsync(CommonOrder? orderCreateEvent)
        {
            if (orderCreateEvent == null)
            {
                return false;
            }

            var updateOrderedBooksCommand = new UpdateOrderedBooksCommand(orderCreateEvent);

            var response = await _eventBus.SendCommand(updateOrderedBooksCommand);

            if (!response)
            {//if rabbitmq not available then send by httpClient synchroniously
                response = await _synchronousSender.UpdateDataAsync<CommonOrder>(orderCreateEvent, _config["MicroRabbitOrders:OrderedBooksApi"]!);
            }

            return response;
        }
    }
}