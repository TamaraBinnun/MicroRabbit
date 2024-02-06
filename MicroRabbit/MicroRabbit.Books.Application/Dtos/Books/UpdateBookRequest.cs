using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MicroRabbit.Domain.Core.Models;

namespace MicroRabbit.Books.Application.Dtos.Books
{
    public class UpdateBookRequest : UpdateBaseRequest
    {
        [Required]
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