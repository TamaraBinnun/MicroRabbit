using MicroRabbit.Application.Interfaces;
using MicroRabbit.Orders.Application.Dtos.OrderItems;
using MicroRabbit.Orders.Domain.Models;

namespace MicroRabbit.Orders.Application.Interfaces
{
    public interface IOrderItemsService : IService<OrderItemResponse, AddOrderItemRequest, UpdateOrderItemRequest>
    {
        Task<IEnumerable<OrderItemResponse>> GetByOrderIdAsync(int orderId);
    }
}