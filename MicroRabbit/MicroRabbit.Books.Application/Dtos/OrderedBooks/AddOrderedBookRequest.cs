using System.ComponentModel.DataAnnotations;

namespace MicroRabbit.Books.Application.Dtos.OrderedBooks
{
    public class AddOrderedBookRequest
    {
        public int OrderId { get; set; }
        public int OrderItemId { get; set; }

        [Required]
        public string ISBN { get; set; } = null!;

        public int ExternalBookId { get; set; }

        public int OrderedUnits { get; set; }
    }
}