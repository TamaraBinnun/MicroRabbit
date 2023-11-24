using AutoMapper;
using MediatR;
using MicroRabbit.Application.Services;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Domain.Core.Dtos;
using MicroRabbit.Orders.Application.Dtos.OrderItems;
using MicroRabbit.Orders.Application.Dtos.Orders;
using MicroRabbit.Orders.Application.Interfaces;
using MicroRabbit.Orders.Domain.Commands;
using MicroRabbit.Orders.Domain.Interfaces;
using MicroRabbit.Orders.Domain.Models;

namespace MicroRabbit.Orders.Application.Services
{
    public class OrderService : Service<Order, OrderResponse, AddOrderRequest, UpdateOrderRequest>, IOrderService
    {
        private readonly IOrderRepository _repository;
        private readonly IEventBus _eventBus;
        private readonly IMapper _mapper;
        private readonly IMicroRabbitBooksClient _booksClient;

        public OrderService(IOrderRepository repository,
                            IEventBus eventBus,
                            IMapper mapper,
                            IMicroRabbitBooksClient updateInStock) : base(repository, eventBus, mapper)
        {
            _repository = repository;
            _eventBus = eventBus;
            _mapper = mapper;
            _booksClient = updateInStock;
        }

        public async Task<IEnumerable<BookUnits>?> GetBookUnitsInStockAsync(List<int> bookIds)
        {
            var response = await _booksClient.GetBookUnitsInStockAsync(bookIds);
            return response;
        }

        public async Task<bool> UpdateBookUnitsInStockAsync(List<OrderItemResponse> orderItems)
        {
            var bookUnits = _mapper.Map<List<BookUnits>>(orderItems);
            var updateStockCommand = new UpdateStockCommand(bookUnits);

            var response = await _eventBus.SendCommand(updateStockCommand);

            if (!response)
            {//if rabbitmq not available then send by http
                response = await _booksClient.UpdateBookUnitsInStockAsync(bookUnits);
            }
            return response;
        }
    }
}