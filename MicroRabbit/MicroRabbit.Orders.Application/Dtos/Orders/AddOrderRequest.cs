namespace MicroRabbit.Orders.Application.Dtos.Orders
{
    public class AddOrderRequest
    {
        public int UserId { get; set; }

        public string DeliveryAddress { get; set; } = null!;
    }
}