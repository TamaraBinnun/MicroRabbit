using MicroRabbit.Orders.Application.Dtos.OrderItems;
using MicroRabbit.Orders.Application.Dtos.Orders;
using System.ComponentModel.DataAnnotations;

namespace MicroRabbit.Orders.Application.Dtos.OrderBooks
{
    public class UpdateOrderBookRequest
    {
        [Required]
        public UpdateOrderRequest Order { get; set; } = null!;

        public IEnumerable<AddOrderItemRequest>? AddOrderItems { get; set; } = null;

        public IEnumerable<UpdateOrderItemRequest>? UpdateOrderItems { get; set; } = null;

        public IEnumerable<int>? DeleteOrderItems { get; set; } = null;
    }
}