namespace MicroRabbit.Books.Application.Dtos
{
    public class UpdateBookRequest
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public int PublicationId { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; } = null!;

        public string CoverImage { get; set; } = null!;
    }
}