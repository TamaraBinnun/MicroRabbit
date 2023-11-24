using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroRabbit.Orders.Domain.Models
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int BookId { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal UnitPrice { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastUpdatedDate { get; set; }
    }
}