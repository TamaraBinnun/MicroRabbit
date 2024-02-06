using MicroRabbit.Domain.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroRabbit.Orders.Domain.Models
{
    [Index(nameof(OrderId), nameof(BookId), IsUnique = true)]
    public class OrderItem : BaseModel
    {
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;//one to many relationship: each order can have many order items

        public int BookId { get; set; }//one to many relationship: each order can have many books
        public Book Book { get; set; } = null!;

        public int OrderedUnits { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal UnitPrice { get; set; }
    }
}