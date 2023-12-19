namespace MicroRabbit.Books.Application.Dtos.OrderedBooks
{
    public class UpdateOrderedBookRequest
    {
        public int Id { get; set; }

        public int OrderItemId { get; set; }

        public int BookId { get; set; }

        public int UnitsBeforeOrder { get; set; }

        public int OrderedUnits { get; set; }
    }
}