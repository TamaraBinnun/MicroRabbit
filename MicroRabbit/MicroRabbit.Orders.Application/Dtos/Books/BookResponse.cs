namespace MicroRabbit.Orders.Application.Dtos
{
    public class BookResponse
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public int ExternalId { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}