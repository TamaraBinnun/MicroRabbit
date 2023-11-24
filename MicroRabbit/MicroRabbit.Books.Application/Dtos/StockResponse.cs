namespace MicroRabbit.Books.Application.Dtos
{
    public class StockResponse
    {
        public int Id { get; set; }

        public int BookId { get; set; }

        public int Units { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}