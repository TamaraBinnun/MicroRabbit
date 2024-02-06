using MicroRabbit.Domain.Core.Models;
using System.Text.Json.Serialization;

namespace MicroRabbit.Orders.Application.Dtos.OrderItems
{
    public class OrderItemResponse : BaseResponse
    {
        [JsonIgnore]
        public int OrderId { get; set; }

        public int BookId { get; set; }

        public int OrderedUnits { get; set; }

        public decimal UnitPrice { get; set; }
    }
}