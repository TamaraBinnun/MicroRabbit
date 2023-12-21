using AutoMapper;
using MicroRabbit.Application.Services;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Orders.Application.Dtos.OrderItems;
using MicroRabbit.Orders.Application.Interfaces;
using MicroRabbit.Orders.Domain.Interfaces;
using MicroRabbit.Orders.Domain.Models;

namespace MicroRabbit.Orders.Application.Services
{
    public class OrderItemsService : Service<OrderItem, OrderItemResponse, AddOrderItemRequest, UpdateOrderItemRequest>, IOrderItemsService
    {
        private readonly IOrderItemsRepository _repository;
        private readonly IEventBus _eventBus;
        private readonly IMapper _mapper;

        public OrderItemsService(IOrderItemsRepository repository,
                            IEventBus eventBus,
                            IMapper mapper) : base(repository, eventBus, mapper)
        {
            _repository = repository;
            _eventBus = eventBus;
            _mapper = mapper;
        }

        public IEnumerable<OrderItemResponse> GetByOrderId(int orderId)
        {
            var orderItems = _repository.GetMany(
                filter: b => b.OrderId == orderId,
                orderBy: b => b.OrderBy(b => b.BookId));

            return _mapper.Map<IEnumerable<OrderItemResponse>>(orderItems);
        }
    }
}