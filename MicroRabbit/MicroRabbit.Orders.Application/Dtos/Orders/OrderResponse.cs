using MicroRabbit.Orders.Application.Dtos.OrderItems;

namespace MicroRabbit.Orders.Application.Dtos.Orders
{
    public class OrderResponse
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int OrderStatusId { get; set; }

        private List<OrderItemResponse> OrderItems { get; set; } = new List<OrderItemResponse>();
    }
}