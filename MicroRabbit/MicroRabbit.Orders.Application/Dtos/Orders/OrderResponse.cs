using MicroRabbit.Domain.Core.Models;
using MicroRabbit.Orders.Application.Dtos.OrderItems;

namespace MicroRabbit.Orders.Application.Dtos.Orders
{
    public class OrderResponse : BaseResponse
    {
        public int UserId { get; set; }

        public int OrderStatus { get; set; }

        public string DeliveryAddress { get; set; } = null!;

        public IEnumerable<OrderItemResponse>? OrderItems { get; set; }
    }
}