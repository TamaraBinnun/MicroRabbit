using MicroRabbit.Domain.Core.Dtos;
using MicroRabbit.Orders.Application.Dtos.OrderBooks;
using MicroRabbit.Orders.Application.Dtos.OrderItems;

namespace MicroRabbit.Orders.Application.Interfaces
{
    public interface IOrderBooksService
    {
        Task<IEnumerable<OrderBookResponse>> GetAllAsync();

        Task<OrderBookResponse?> GetByOrderIdAsync(int orderId);

        Task<OrderBookResponse> AddAsync(AddOrderBookRequest addRequest);

        Task<int> UpdateAsync(UpdateOrderBookRequest updateRequest);

        Task<int> DeleteAsync(int id);
    }
}