namespace MicroRabbit.Books.Application.Dtos
{
    public class BookResponse
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public int PublicationId { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; } = null!;

        public string CoverImage { get; set; } = null!;

        public DateTime CreatedDate { get; set; }
    }
}