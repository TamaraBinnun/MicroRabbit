using System.ComponentModel.DataAnnotations;

namespace MicroRabbit.Domain.Core.Dtos
{
    public class CommonOrderedBook
    {
        public int OrderItemId { get; set; }

        [Required]
        public string ISBN { get; set; } = null!;

        public int BookId { get; set; }

        public int OrderedUnits { get; set; }
    }
}