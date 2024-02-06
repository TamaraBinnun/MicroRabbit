using MicroRabbit.Application.Interfaces;
using MicroRabbit.Orders.Application.Dtos.OrderItems;

namespace MicroRabbit.Orders.Application.Interfaces
{
    public interface IOrderItemsService : IService<OrderItemResponse, AddOrderItemRequest, UpdateOrderItemRequest>
    {
    }
}