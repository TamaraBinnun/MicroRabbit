namespace MicroRabbit.Orders.Application.Dtos.Orders
{
    public class OrderResponse
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int OrderStatusId { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}