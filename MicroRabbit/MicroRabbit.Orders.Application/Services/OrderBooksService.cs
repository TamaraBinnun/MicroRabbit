using AutoMapper;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Domain.Core.Dtos;
using MicroRabbit.Domain.Core.Interfaces;
using MicroRabbit.Orders.Application.Dtos.OrderBooks;
using MicroRabbit.Orders.Application.Dtos.OrderItems;
using MicroRabbit.Orders.Application.Dtos.Orders;
using MicroRabbit.Orders.Application.Interfaces;
using MicroRabbit.Orders.Domain.Commands;
using MicroRabbit.Orders.Domain.Models;
using Microsoft.Extensions.Configuration;
using System.Reflection.Metadata;

namespace MicroRabbit.Orders.Application.Services
{
    public class OrderBooksService : IOrderBooksService
    {
        private readonly IOrdersService _orderService;
        private readonly IOrderItemsService _orderItemsService;
        private readonly IEventBus _eventBus;
        private readonly IMapper _mapper;
        private readonly ISynchronousSender _synchronousSender;
        private readonly IConfiguration _config;

        public OrderBooksService(IOrdersService orderService, IOrderItemsService orderItemsService,
                            IEventBus eventBus,
                            IMapper mapper,
                            ISynchronousSender synchronousSender,
                            IConfiguration config)
        {
            _orderService = orderService;
            _orderItemsService = orderItemsService;
            _eventBus = eventBus;
            _mapper = mapper;
            _synchronousSender = synchronousSender;
            _config = config;
        }

        public async Task<IEnumerable<OrderBookResponse>> GetAllAsync()
        {
            var orders = await _orderService.GetAllAsync();

            var tasks = orders.Select(async order => new OrderBookResponse()
            {
                Order = order,
                OrderItems = await _orderItemsService.GetByOrderIdAsync(order.Id),
            });

            var response = await Task.WhenAll(tasks);

            return response;
        }

        public async Task<OrderBookResponse?> GetByOrderIdAsync(int orderId)
        {
            var response = new OrderBookResponse()
            {
                Order = await _orderService.GetByIdAsync(orderId),
                OrderItems = await _orderItemsService.GetByOrderIdAsync(orderId),
            };

            return response;
        }

        public async Task<OrderBookResponse> AddAsync(AddOrderBookRequest addRequest)
        {
            var response = new OrderBookResponse()
            {
                Order = await _orderService.AddAsync(addRequest.Order),
                OrderItems = await _orderItemsService.AddManyAsync(addRequest.OrderItems)
            };

            #region CreateEvent

            var commonOrderedBooks = _mapper.Map<IEnumerable<CommonOrderedBook>>(response.OrderItems);
            await CreateEventToUpdateOrderedBooksAsync(commonOrderedBooks);

            #endregion CreateEvent

            return response;
        }

        public async Task<int> UpdateAsync(UpdateOrderBookRequest updateRequest)
        {
            var orderResponse = await _orderService.UpdateAsync(updateRequest.Order);
            if (orderResponse <= 0)
            {
                return orderResponse;
            }

            var addedOrderItems = await HandleAddOrderItems(updateRequest.AddOrderItems);
            var updatedOrderItems = await HandleUpdateOrderItems(updateRequest.UpdateOrderItems);
            var deletedOrderItems = await HandleDeleteOrderItems(updateRequest.DeleteOrderItems);

            #region CreateEvent

            var orderItemsToCreateEvent = new[] { addedOrderItems, updatedOrderItems, deletedOrderItems }.Where(x => x != null).SelectMany(x => x!);

            await CreateEventToUpdateOrderedBooksAsync(orderItemsToCreateEvent);

            #endregion CreateEvent

            return orderResponse;
        }

        public async Task<int> DeleteAsync(int orderId)
        {
            #region CreateEvent

            //get and save orderItems and save before deleting them in order to create event
            var orderItems = await _orderItemsService.GetByOrderIdAsync(orderId);

            #endregion CreateEvent

            var orderItemsResponse = await _orderItemsService.DeleteManyByFilterAsync(filter: x => x.OrderId == orderId);

            var orderResponse = await _orderService.DeleteAsync(orderId);

            #region CreateEvent

            if (orderItems != null)
            {
                var commonOrderedBooks = _mapper.Map<IEnumerable<CommonOrderedBook>>(orderItems);
                commonOrderedBooks.ToList().ForEach(x => x.IsDeleted = true);
                await CreateEventToUpdateOrderedBooksAsync(commonOrderedBooks);
            }

            #endregion CreateEvent

            return orderResponse;
        }

        private async Task<IEnumerable<CommonOrderedBook>?> HandleAddOrderItems(IEnumerable<AddOrderItemRequest>? addOrderItems)
        {
            if (addOrderItems == null)
            {
                return null;
            }

            var addedOrderItems = await _orderItemsService.AddManyAsync(addOrderItems);

            #region CreateEvent

            var commonOrderedBooks = _mapper.Map<IEnumerable<CommonOrderedBook>>(addedOrderItems);
            return commonOrderedBooks;

            #endregion CreateEvent
        }

        private async Task<IEnumerable<CommonOrderedBook>?> HandleUpdateOrderItems(IEnumerable<UpdateOrderItemRequest>? updateOrderItems)
        {
            if (updateOrderItems == null)
            {
                return null;
            }

            await _orderItemsService.UpdateManyAsync(updateOrderItems);

            #region CreateEvent

            var commonOrderedBooks = _mapper.Map<IEnumerable<CommonOrderedBook>>(updateOrderItems);
            return commonOrderedBooks;

            #endregion CreateEvent
        }

        private async Task<IEnumerable<CommonOrderedBook>?> HandleDeleteOrderItems(IEnumerable<int>? deleteOrderItems)
        {
            if (deleteOrderItems == null)
            {
                return null;
            }

            #region CreateEvent

            var orderItemsToDelete = await _orderItemsService.GetManyByIdAsync(deleteOrderItems);

            #endregion CreateEvent

            await _orderItemsService.DeleteManyAsync(deleteOrderItems);

            #region CreateEvent

            var commonOrderedBooks = _mapper.Map<IEnumerable<CommonOrderedBook>>(orderItemsToDelete);
            commonOrderedBooks.ToList().ForEach(x => x.IsDeleted = true);
            return commonOrderedBooks;

            #endregion CreateEvent
        }

        private async Task<bool> CreateEventToUpdateOrderedBooksAsync(IEnumerable<CommonOrderedBook>? commonOrderedBooks)
        {
            if (commonOrderedBooks == null)
            {
                return false;
            }

            var updateOrderedBooksCommand = new UpdateOrderedBooksCommand(commonOrderedBooks);

            var response = await _eventBus.SendCommand(updateOrderedBooksCommand);

            if (!response)
            {//if rabbitmq not available then send by httpClient synchroniously
                response = await _synchronousSender.UpdateDataAsync<IEnumerable<CommonOrderedBook>>(commonOrderedBooks, _config["MicroRabbitOrders:OrderedBooksApi"]!);
            }
            return response;
        }
    }
}