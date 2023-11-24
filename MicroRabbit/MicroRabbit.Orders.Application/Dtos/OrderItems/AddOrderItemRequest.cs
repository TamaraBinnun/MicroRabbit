namespace MicroRabbit.Orders.Application.Dtos.OrderItems
{
    public class AddOrderItemRequest
    {
        public int OrderId { get; set; }

        public int BookId { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }
    }
}