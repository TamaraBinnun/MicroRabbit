namespace MicroRabbit.Orders.Application.Dtos.OrderItems
{
    public class UpdateOrderItemRequest
    {
        public int Id { get; set; }

        public int OrderedUnits { get; set; }

        public decimal UnitPrice { get; set; }
    }
}