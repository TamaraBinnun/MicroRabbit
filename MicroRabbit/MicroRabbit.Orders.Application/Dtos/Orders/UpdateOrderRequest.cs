namespace MicroRabbit.Orders.Application.Dtos.Orders
{
    public class UpdateOrderRequest
    {
        public int Id { get; set; }

        public int OrderStatusId { get; set; }
    }
}