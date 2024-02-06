using MicroRabbit.Domain.Core.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroRabbit.Orders.Domain.Models
{
    public class Payment : BaseModel
    {
        public int OrderId { get; set; }//one to one relationship: each order can have one payment data
        public Order Order { get; set; } = null!;

        [Column(TypeName = "decimal(5, 2)")]
        public decimal Total { get; set; }

        public string? Remarks { get; set; }
    }
}