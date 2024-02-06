using System.Text.Json.Serialization;

namespace MicroRabbit.Orders.Application.Dtos.OrderItems
{
    public class AddOrderItemRequest
    {
        public int BookId { get; set; }

        public int OrderedUnits { get; set; }

        public decimal UnitPrice { get; set; }
    }
}