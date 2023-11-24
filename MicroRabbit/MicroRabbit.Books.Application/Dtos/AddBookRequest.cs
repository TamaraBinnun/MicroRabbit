using System.ComponentModel.DataAnnotations;

namespace MicroRabbit.Books.Application.Dtos
{
    public class AddBookRequest
    {
        [Required]
        public string Name { get; set; } = null!;

        public int PublicationId { get; set; }

        public int CategoryId { get; set; }

        public decimal Price { get; set; }

        [Required]
        public string Description { get; set; } = null!;

        [Required]
        public string CoverImage { get; set; } = null!;
    }
}