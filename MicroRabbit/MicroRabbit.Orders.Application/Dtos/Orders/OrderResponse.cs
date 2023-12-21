namespace MicroRabbit.Orders.Application.Dtos.Orders
{
    public class OrderResponse
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int OrderStatus { get; set; }

        public string DeliveryAddress { get; set; } = null!;

        public DateTime CreatedDate { get; set; }
    }
}