using System.ComponentModel.DataAnnotations;

namespace MicroRabbit.Orders.Domain.Models
{
    public class OrderStatus
    {
        [Key]
        public int Id { get; set; }

        public int Status { get; set; }

        [Required]
        [MaxLength(50)]
        public string Description { get; set; } = null!;

        public DateTime CreatedDate { get; set; }

        public DateTime LastUpdatedDate { get; set; }
    }
}