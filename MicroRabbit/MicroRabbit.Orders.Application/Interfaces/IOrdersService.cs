using MicroRabbit.Application.Interfaces;
using MicroRabbit.Domain.Core.Dtos;
using MicroRabbit.Orders.Application.Dtos.Orders;

namespace MicroRabbit.Orders.Application.Interfaces
{
    public interface IOrdersService : IService<OrderResponse, AddOrderRequest, UpdateOrderRequest>
    {
        Task<OrderResponse?> UpdateOrder(int id, UpdateOrderRequest updateOrderRequest);

        Task<bool> CreateEventToUpdateOrderedBooksAsync(CommonOrder? orderCreateEvent);
    }
}