using MicroRabbit.Domain.Core.Models;

namespace MicroRabbit.Books.Application.Dtos.OrderedBooks
{
    public class OrderedBookResponse : BaseResponse
    {
        public int OrderItemId { get; set; }

        public string ISBN { get; set; } = null!;

        public int? BookId { get; set; }

        public int ExternalBookId { get; set; }

        public int OrderedUnits { get; set; }

        public bool IsStockUpdated { get; set; }
    }
}