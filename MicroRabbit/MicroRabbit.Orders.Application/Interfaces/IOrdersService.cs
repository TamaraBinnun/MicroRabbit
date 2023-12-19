using MicroRabbit.Application.Interfaces;
using MicroRabbit.Orders.Application.Dtos.Orders;

namespace MicroRabbit.Orders.Application.Interfaces
{
    public interface IOrdersService : IService<OrderResponse, AddOrderRequest, UpdateOrderRequest>
    {
    }
}