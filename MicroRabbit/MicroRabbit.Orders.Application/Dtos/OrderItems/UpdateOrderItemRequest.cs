using MicroRabbit.Domain.Core.Models;

namespace MicroRabbit.Orders.Application.Dtos.OrderItems
{
    public class UpdateOrderItemRequest : UpdateBaseRequest
    {
        public int OrderedUnits { get; set; }

        public decimal UnitPrice { get; set; }
    }
}