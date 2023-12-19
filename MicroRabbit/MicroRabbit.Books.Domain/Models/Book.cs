using MicroRabbit.Domain.Core.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroRabbit.Books.Domain.Models
{
    public class Book : BaseModel
    {
        [Required]
        [MaxLength(50)]
        public string Title { get; set; } = null!;

        [Required]
        public int PublicationId { get; set; }

        [Required]
        public int AuthorId { get; set; }

        [Required]
        [Column(TypeName = "decimal(5, 2)")]
        public decimal Price { get; set; }

        [Required]
        [MaxLength(300)]
        public string Description { get; set; } = null!;

        [Required]
        public string CoverImage { get; set; } = null!;

        public int Units { get; set; }
    }
}