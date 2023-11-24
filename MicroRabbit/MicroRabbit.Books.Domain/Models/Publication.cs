using System.ComponentModel.DataAnnotations;

namespace MicroRabbit.Books.Domain.Models
{
    public class Publication
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string Description { get; set; } = null!;

        public DateTime CreatedDate { get; set; }

        public DateTime LastUpdatedDate { get; set; }
    }
}