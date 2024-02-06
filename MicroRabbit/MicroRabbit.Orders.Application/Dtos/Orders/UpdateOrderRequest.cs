using MicroRabbit.Domain.Core.Models;
using MicroRabbit.Orders.Application.Dtos.OrderItems;
using MicroRabbit.Orders.Domain.Models;
using System.Text.Json.Serialization;

namespace MicroRabbit.Orders.Application.Dtos.Orders
{
    public class UpdateOrderRequest : UpdateBaseRequest
    {
        public string DeliveryAddress { get; set; } = null!;

        [JsonIgnore]
        public OrderStatus OrderStatus { get; set; }

        //always passing all order items
        //will find by book id if new (add) or existing book (check if has changes to update)
        //will delete not existing
        public IEnumerable<AddOrderItemRequest>? OrderItems { get; set; } = null!;
    }
}