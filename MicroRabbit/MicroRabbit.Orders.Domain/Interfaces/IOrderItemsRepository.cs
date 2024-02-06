using MicroRabbit.Domain.Core.Interfaces;
using MicroRabbit.Domain.Core.Models;
using MicroRabbit.Orders.Domain.Models;

namespace MicroRabbit.Orders.Domain.Interfaces
{
    public interface IOrderItemsRepository<UpdateTRequest> : IRepository<OrderItem, UpdateTRequest>
         where UpdateTRequest : UpdateBaseRequest
    {
    }
}