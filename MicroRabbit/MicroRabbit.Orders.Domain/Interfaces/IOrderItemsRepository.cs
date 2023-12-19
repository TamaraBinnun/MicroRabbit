using MicroRabbit.Domain.Core.Interfaces;
using MicroRabbit.Orders.Domain.Models;

namespace MicroRabbit.Orders.Domain.Interfaces
{
    public interface IOrderItemsRepository : IRepository<OrderItem>
    {
    }
}