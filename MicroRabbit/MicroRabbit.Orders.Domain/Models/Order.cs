using MicroRabbit.Domain.Core.Models;

namespace MicroRabbit.Orders.Domain.Models
{
    public class Order : BaseModel
    {
        public int UserId { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public string DeliveryAddress { get; set; } = null!;

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}