using MicroRabbit.Application.Interfaces;
using MicroRabbit.Domain.Core.Dtos;
using MicroRabbit.Orders.Application.Dtos.OrderItems;
using MicroRabbit.Orders.Application.Dtos.Orders;

namespace MicroRabbit.Orders.Application.Interfaces
{
    public interface IOrderService : IService<OrderResponse, AddOrderRequest, UpdateOrderRequest>
    {
        Task<IEnumerable<BookUnits>?> GetBookUnitsInStockAsync(List<int> bookIds);

        Task<bool> UpdateBookUnitsInStockAsync(List<OrderItemResponse> orderItems);
    }
}