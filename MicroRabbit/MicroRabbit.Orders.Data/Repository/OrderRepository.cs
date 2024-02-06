using MicroRabbit.Data.Repository;
using MicroRabbit.Domain.Core.Models;
using MicroRabbit.Orders.Data.Context;
using MicroRabbit.Orders.Domain.Interfaces;
using MicroRabbit.Orders.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroRabbit.Orders.Data.Repository
{
    public class OrderRepository<UpdateTRequest, UpdateTListRequest> : Repository<Order, UpdateTRequest>,
                                                   IOrdersRepository<UpdateTRequest, UpdateTListRequest>
        where UpdateTRequest : UpdateBaseRequest
        where UpdateTListRequest : UpdateBaseRequest
    {
        private readonly OrderDbContext _context;
        private readonly DbSet<OrderItem> _dbOrderItemSet;

        public OrderRepository(OrderDbContext context) : base(context)
        {
            _context = context;
            _dbOrderItemSet = _context.Set<OrderItem>();
        }

        public void UpdateOrderItems(IEnumerable<(OrderItem currentOrderItem, UpdateTListRequest updateOrderItemRequest)>? updateRequest)
        {
            if (updateRequest == null) { return; }

            updateRequest.ToList().ForEach(x =>
            {
                x.updateOrderItemRequest.LastUpdatedDate = DateTime.Now;

                _context.Entry(x.currentOrderItem).CurrentValues.SetValues(x.updateOrderItemRequest);
            });
        }

        public void AddOrderItemRange(IEnumerable<OrderItem>? entities)
        {
            if (entities == null) { return; }

            _dbOrderItemSet.AddRange(entities);
        }

        public void DeleteOrderItemRange(IEnumerable<OrderItem>? entities)
        {
            if (entities == null) { return; }

            _dbOrderItemSet.RemoveRange(entities);
        }
    }
}