using MicroRabbit.Domain.Core.Models;

namespace MicroRabbit.Books.Application.Dtos.Books
{
    public class BookResponse : BaseResponse
    {
        public string ISBN { get; set; } = null!;

        public string Title { get; set; } = null!;

        public int PublicationId { get; set; }

        public int AuthorId { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; } = null!;

        public string CoverImage { get; set; } = null!;

        public int Units { get; set; }
    }
}