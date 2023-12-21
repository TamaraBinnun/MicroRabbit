namespace MicroRabbit.Orders.Application.Dtos.Orders
{
    public class UpdateOrderRequest
    {
        public int Id { get; set; }

        public string DeliveryAddress { get; set; } = null!;
    }
}