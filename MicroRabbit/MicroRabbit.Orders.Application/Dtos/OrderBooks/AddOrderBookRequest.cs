using MicroRabbit.Orders.Application.Dtos.OrderItems;
using MicroRabbit.Orders.Application.Dtos.Orders;
using System.ComponentModel.DataAnnotations;

namespace MicroRabbit.Orders.Application.Dtos.OrderBooks
{
    public class AddOrderBookRequest
    {
        [Required]
        public AddOrderRequest Order { get; set; } = null!;

        [Required]
        public IEnumerable<AddOrderItemRequest> OrderItems { get; set; } = null!;
    }
}