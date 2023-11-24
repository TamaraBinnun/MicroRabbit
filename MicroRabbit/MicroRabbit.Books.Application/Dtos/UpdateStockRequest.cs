namespace MicroRabbit.Books.Domain.Models
{
    public class UpdateStockRequest
    {
        public int Id { get; set; }

        public int BookId { get; set; }

        public int Units { get; set; }
    }
}