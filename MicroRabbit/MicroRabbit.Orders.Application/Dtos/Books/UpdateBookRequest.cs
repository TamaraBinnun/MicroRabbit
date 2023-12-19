namespace MicroRabbit.Orders.Application.Dtos.Books
{
    public class UpdateBookRequest
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public int ExternalId { get; set; }
    }
}