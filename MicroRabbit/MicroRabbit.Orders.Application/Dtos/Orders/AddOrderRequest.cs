using MicroRabbit.Orders.Application.Dtos.OrderItems;
using System.ComponentModel.DataAnnotations;

namespace MicroRabbit.Orders.Application.Dtos.Orders
{
    public class AddOrderRequest
    {
        public int UserId { get; set; }

        [Required]
        public string DeliveryAddress { get; set; } = null!;

        [Required]
        public IEnumerable<AddOrderItemRequest> OrderItems { get; set; } = null!;
    }
}