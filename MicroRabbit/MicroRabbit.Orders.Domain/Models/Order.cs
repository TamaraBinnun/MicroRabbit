using MicroRabbit.Domain.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace MicroRabbit.Orders.Domain.Models
{
    public class Order : BaseModel
    {
        public int UserId { get; set; }

        public int OrderStatusId { get; set; }
    }
}