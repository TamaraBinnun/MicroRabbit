namespace MicroRabbit.Orders.Application.Dtos.Books
{
    public class AddBookRequest
    {
        public string ISBN { get; set; } = null!;

        public string Title { get; set; } = null!;

        public int ExternalId { get; set; }
    }
}