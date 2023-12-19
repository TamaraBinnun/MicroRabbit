using AutoMapper;
using MicroRabbit.Application.Services;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Orders.Application.Dtos.Orders;
using MicroRabbit.Orders.Application.Interfaces;
using MicroRabbit.Orders.Domain.Interfaces;
using MicroRabbit.Orders.Domain.Models;

namespace MicroRabbit.Orders.Application.Services
{
    public class OrdersService : Service<Order, OrderResponse, AddOrderRequest, UpdateOrderRequest>, IOrdersService
    {
        private readonly IOrdersRepository _repository;
        private readonly IEventBus _eventBus;
        private readonly IMapper _mapper;

        public OrdersService(IOrdersRepository repository,
                            IEventBus eventBus,
                            IMapper mapper) : base(repository, eventBus, mapper)
        {
            _repository = repository;
            _eventBus = eventBus;
            _mapper = mapper;
        }
    }
}