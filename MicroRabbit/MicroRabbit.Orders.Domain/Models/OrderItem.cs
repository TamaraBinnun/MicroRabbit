using MicroRabbit.Domain.Core.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroRabbit.Orders.Domain.Models
{
    public class OrderItem : BaseModel
    {
        public int OrderId { get; set; }

        public int BookId { get; set; }

        public int OrderedUnits { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal UnitPrice { get; set; }

        public Book Book { get; set; } = null!;
    }
}