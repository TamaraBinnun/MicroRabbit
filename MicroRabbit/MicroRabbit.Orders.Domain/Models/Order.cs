using System.ComponentModel.DataAnnotations;

namespace MicroRabbit.Orders.Domain.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public int OrderStatusId { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastUpdatedDate { get; set; }
    }
}