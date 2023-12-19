using MicroRabbit.Domain.Core.Models;

namespace MicroRabbit.Books.Domain.Models
{
    public class OrderedBook : BaseModel
    {
        public int OrderId { get; set; }
        public int OrderItemId { get; set; }

        public int BookId { get; set; }

        public int OrderedUnits { get; set; }

        public bool IsStockUpdated { get; set; }
    }
}