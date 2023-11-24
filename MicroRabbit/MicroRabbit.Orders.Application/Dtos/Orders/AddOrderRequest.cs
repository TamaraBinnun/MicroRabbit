using MicroRabbit.Orders.Application.Dtos.OrderItems;
using System.ComponentModel.DataAnnotations;

namespace MicroRabbit.Orders.Application.Dtos.Orders
{
    public class AddOrderRequest
    {
        public int UserId { get; set; }

        public List<OrderItemResponse> OrderItems { get; set; } = null!;
    }
}