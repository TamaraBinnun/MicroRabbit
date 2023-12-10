using System.ComponentModel.DataAnnotations;

namespace MicroRabbit.Orders.Domain.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        public int ExternalId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Title { get; set; } = null!;

        public DateTime CreatedDate { get; set; }

        public DateTime LastUpdatedDate { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}