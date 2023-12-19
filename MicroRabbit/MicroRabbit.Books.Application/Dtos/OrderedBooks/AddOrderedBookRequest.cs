namespace MicroRabbit.Books.Application.Dtos.OrderedBooks
{
    public class AddOrderedBookRequest
    {
        public int OrderId { get; set; }
        public int OrderItemId { get; set; }

        public int BookId { get; set; }

        public int OrderedUnits { get; set; }
    }
}