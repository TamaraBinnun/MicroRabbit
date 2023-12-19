using MicroRabbit.Orders.Application.Dtos.OrderItems;
using MicroRabbit.Orders.Application.Dtos.Orders;

namespace MicroRabbit.Orders.Application.Dtos.OrderBooks
{
    public class OrderBookResponse
    {
        public OrderResponse? Order { get; set; } = null!;

        public IEnumerable<OrderItemResponse> OrderItems { get; set; } = null!;
    }
}