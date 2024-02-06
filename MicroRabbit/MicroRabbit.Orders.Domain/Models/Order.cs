using MicroRabbit.Domain.Core.Models;

namespace MicroRabbit.Orders.Domain.Models
{
    public class Order : BaseModel
    {
        public int UserId { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public string DeliveryAddress { get; set; } = null!;

        public int? PaymentId { get; set; }//one to one relationship: each order ca have one payment data
        public Payment? Payment { get; set; }

        public ICollection<OrderItem>? OrderItems { get; set; }//one to many relationship: each order can have many order items
    }
}