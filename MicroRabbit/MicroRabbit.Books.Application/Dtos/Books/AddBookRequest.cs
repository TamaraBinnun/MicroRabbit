using System.ComponentModel.DataAnnotations;

namespace MicroRabbit.Books.Application.Dtos.Books
{
    public class AddBookRequest
    {
        [Required]
        public string Title { get; set; } = null!;

        public int PublicationId { get; set; }

        public int AuthorId { get; set; }

        public int CategoryId { get; set; }

        public decimal Price { get; set; }

        [Required]
        public string Description { get; set; } = null!;

        [Required]
        public string CoverImage { get; set; } = null!;

        public int Units { get; set; }
    }
}