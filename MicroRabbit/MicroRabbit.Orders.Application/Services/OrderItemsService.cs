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
        private readonly IOrderItemsRepository<UpdateOrderItemRequest> _repository;
        private readonly IEventBus _eventBus;
        private readonly IMapper _mapper;

        public OrderItemsService(IOrderItemsRepository<UpdateOrderItemRequest> repository,
                                 IEventBus eventBus,
                                 IMapper mapper) : base(repository, eventBus, mapper)
        {
            _repository = repository;
            _eventBus = eventBus;
            _mapper = mapper;
        }
    }
}