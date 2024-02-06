using MicroRabbit.Domain.Core.Models;

namespace MicroRabbit.Books.Application.Dtos.OrderedBooks
{
    public class UpdateOrderedBookRequest : UpdateBaseRequest
    {
        public int OrderItemId { get; set; }

        public int OrderedUnits { get; set; }
    }
}