using MicroRabbit.Domain.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MicroRabbit.Books.Domain.Models
{
    [Index(nameof(OrderId), nameof(BookId), IsUnique = true)]
    [Index(nameof(OrderId), nameof(ExternalBookId), IsUnique = true)]
    public class OrderedBook : BaseModel
    {
        public int OrderId { get; set; }
        public int OrderItemId { get; set; }

        [MaxLength(20)]
        public string ISBN { get; set; } = null!;

        public int? BookId { get; set; }

        public int ExternalBookId { get; set; }

        public int OrderedUnits { get; set; }

        public bool IsStockUpdated { get; set; }
    }
}