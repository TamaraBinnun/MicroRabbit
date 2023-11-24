namespace MicroRabbit.Orders.Application.Dtos.OrderItems
{
    public class UpdateOrdetItemRequest
    {
        public int Id { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }
    }
}