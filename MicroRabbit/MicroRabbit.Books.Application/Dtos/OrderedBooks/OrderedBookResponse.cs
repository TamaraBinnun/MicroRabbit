namespace MicroRabbit.Books.Application.Dtos.OrderedBooks
{
    public class OrderedBookResponse
    {
        public int Id { get; set; }

        public int OrderItemId { get; set; }

        public int BookId { get; set; }

        public int UnitsBeforeOrder { get; set; }

        public int OrderedUnits { get; set; }

        public bool IsStockUpdated { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}