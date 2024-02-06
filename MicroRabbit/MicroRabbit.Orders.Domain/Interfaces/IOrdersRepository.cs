using MicroRabbit.Domain.Core.Interfaces;
using MicroRabbit.Domain.Core.Models;
using MicroRabbit.Orders.Domain.Models;

namespace MicroRabbit.Orders.Domain.Interfaces
{
    public interface IOrdersRepository<UpdateTRequest, UpdateTListRequest> : IRepository<Order, UpdateTRequest>
         where UpdateTRequest : UpdateBaseRequest
        where UpdateTListRequest : UpdateBaseRequest
    {
        void UpdateOrderItems(IEnumerable<(OrderItem currentOrderItem, UpdateTListRequest updateOrderItemRequest)>? updateRequest);

        void AddOrderItemRange(IEnumerable<OrderItem>? entities);

        void DeleteOrderItemRange(IEnumerable<OrderItem>? entities);
    }
}