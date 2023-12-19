namespace MicroRabbit.Orders.Application.Dtos.OrderItems
{
    public class OrderItemResponse
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int BookId { get; set; }

        public int OrderedUnits { get; set; }

        public decimal UnitPrice { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}